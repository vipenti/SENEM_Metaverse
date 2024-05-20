using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


public class QuestionDispatcher : MonoBehaviour
{ 
    private Queue<Tuple<DateTime, AudioClip>> questions,
                                            teacherExplanations;
    public UnityWebRequest www;

    private GameObject student;

    [Serializable]
    private class TextData{
        public string subject;
    }

    void Start()
    {

        student = GameObject.Find("SmartStudent");
        StartCoroutine(SendTextToServer("Test"));

        StartCoroutine(SendAudioToServer()); 
    }

    public void AddAudioClip(AudioClip clip, DateTime? date = null)
    {
        if (date == null) date = DateTime.Now;

        teacherExplanations.Enqueue(new Tuple<DateTime, AudioClip>((DateTime) date, clip));
    }

    public AudioClip GetQuestion()
    {
        return GetNewerTuple(questions).Item2;
    }

    private IEnumerator SendTextToServer(string text, string url = "http://127.0.0.1:5000/start")
    {  
        Debug.Log("Sending text...");

        var data = new TextData {
            subject = text
        };

        string json = JsonUtility.ToJson(data);

        Debug.Log("JSON: " + json);

        // Convert the JSON string to a byte array
        byte[] bytes = Encoding.UTF8.GetBytes(json);


        www = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(bytes),
            downloadHandler = new DownloadHandlerBuffer()
        };
        
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Text sent!");
            Debug.Log("Response: " + www.downloadHandler.text);
        }
    }

    private IEnumerator SendAudioToServer(string url = "http://127.0.0.1:5000/generate_question")
    {
        while (true)
        {
            Tuple<DateTime, AudioClip> clip = GetNewerTuple(teacherExplanations, 1);

            if (clip != null)
            {
                byte[] bytes = ConvertAudioClipToWav(clip.Item2);

                Debug.Log("Sending audio...");

                www = new UnityWebRequest(url, "POST")
                {
                    uploadHandler = new UploadHandlerRaw(bytes),
                    downloadHandler = new DownloadHandlerBuffer()
                };

                www.SetRequestHeader("Content-Type", "audio/wav");

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Audio recording sent!");
                    Debug.Log("Response: " + www.downloadHandler);

                    // Get the audio data from the response
                    byte[] audioBytes = www.downloadHandler.data;

                    // Convert the byte array to a float array
                    float[] audioDataResponse = new float[audioBytes.Length / 2];

                    for (int i = 0; i < audioBytes.Length; i += 2)
                    {
                        short sample = BitConverter.ToInt16(audioBytes, i);
                        audioDataResponse[i / 2] = sample / 32768.0f;
                    }

                    // Create a new AudioClip and set the audio data
                    AudioClip audioClip = AudioClip.Create("ReceivedAudio", audioDataResponse.Length, 1, 24000, false);
                    audioClip.SetData(audioDataResponse, 0);

                    // when setting audioclip, start event to send audio to student controller
                }
            }

            yield return new WaitForSeconds(5);
        }
    }

    private byte[] ConvertAudioClipToWav(AudioClip clip)
    {
        var hz = clip.frequency;
        var channels = clip.channels;
        var samples = clip.samples;

        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        writer.Write(Encoding.ASCII.GetBytes("RIFF"));
        writer.Write(44 + samples * channels * 2); // File size
        writer.Write(Encoding.ASCII.GetBytes("WAVE"));
        writer.Write(Encoding.ASCII.GetBytes("fmt "));
        writer.Write(16); // Sub chunk size
        writer.Write((short)1); // Audio format (1 = PCM)
        writer.Write((short)channels);
        writer.Write(hz);
        writer.Write(hz * channels * 2);
        writer.Write((short)(channels * 2));
        writer.Write((short)16); // Bits per sample
        writer.Write(Encoding.ASCII.GetBytes("data"));
        writer.Write(samples * channels * 2);

        float[] audioData = new float[samples * channels];
        clip.GetData(audioData, 0);

        for (int i = 0; i < audioData.Length; i++)
        {
            writer.Write((short)(audioData[i] * short.MaxValue));
        }

        byte[] bytes = stream.ToArray();

        writer.Close();
        stream.Close();

        return bytes;
    }

    private Tuple<DateTime, AudioClip> GetNewerTuple(Queue<Tuple<DateTime, AudioClip>> queue, double maxMinutes = 2)
    {
        if (queue.Count == 0) return null;
    
        Tuple<DateTime, AudioClip> tuple = null;

        do
        {
            try
            {
                tuple = teacherExplanations.Dequeue();
            }
            catch (System.Exception)
            {
                return null;
            }

        } while (DateTime.Now - tuple.Item1 > TimeSpan.FromMinutes(maxMinutes));    
    
        return tuple;
    }
}

