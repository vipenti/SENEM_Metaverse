using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class QuestionDispatcher : MonoBehaviour
{ 
    private Queue<Tuple<DateTime, AudioClip>> questions;
    public UnityWebRequest www;

    private GameObject student;

    [Serializable]
    private class TextData
    {
        public string subject;
    }

    [Serializable]
    private class TaskID
    {
        public string result_id;
    }

    [Serializable]
    private class TaskResult
    {
        public bool ready;
        public bool successful;
        //public byte[] value;
        public string value;
    }

    void Start()
    {
        questions = new Queue<Tuple<DateTime, AudioClip>>();

        student = GameObject.Find("SmartStudent");
        // StartCoroutine(SendTextToServer("Test"));
    }

    public void StartStudent(string text)
    {
        StartCoroutine(SendTextToServer(text));
    }

    public void AddAudioClip(AudioClip clip, DateTime? date = null)
    {
        if (date == null) date = DateTime.Now;

        StartCoroutine(SendAudioToServer(new Tuple<DateTime, AudioClip>((DateTime)date, clip)));
    }

    public AudioClip GetQuestion()
    {
        // return GetFreshTuple(questions)?.Item2;
        if (questions.Count > 0) 
        {
            return questions.Dequeue().Item2;
        }

        return null;
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
            Debug.Log("Text arrived!");
            Debug.Log("Response: " + www.downloadHandler.text);
        }

        www.Dispose();
    }

    private IEnumerator SendAudioToServer(Tuple<DateTime, AudioClip> clip, string url = "http://127.0.0.1:5000/generate_question")
    {
        var data = new TaskID();

        // while (true)
        // {

            if (clip != null && clip.Item2 != null)
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
                    Debug.Log("ID arrived!");

                    data = JsonUtility.FromJson<TaskID>(www.downloadHandler.text);

                    string id = data.result_id;

                    Debug.Log("ID: " + id);

                    yield return new WaitForSeconds(30);
                    StartCoroutine(GetAudioFromServer("http://localhost:5000/result/" + id));    
                }

                
            }

        // }
        www.Dispose();
    }

    private IEnumerator GetAudioFromServer(string url = "http://localhost:5000/result/0"){

        int retries = 0;

        while(true){
            Debug.Log("Checking for audio...");
            Debug.Log("URL: " + url);

            // if (www != null) www.Dispose();

            UnityWebRequest www2;

            www2 = new UnityWebRequest(url, "POST")
            {
                downloadHandler = new DownloadHandlerBuffer()
            };
            
            www2.SetRequestHeader("Content-Type", "application/json");

            yield return www2.SendWebRequest();

            if (www2.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www2.error);
            }
            
            else
            {

                Debug.Log("Audio recording arrived!");
                Debug.Log("Response: " + www2.downloadHandler);

                TaskResult taskResult = JsonUtility.FromJson<TaskResult>(www2.downloadHandler.text);

                if (!taskResult.ready && retries < 6)
                {
                    Debug.Log("Task not ready yet, trying again in 10 seconds...");
                    retries++;

                    yield return new WaitForSeconds(10);
                    continue;
                }

                if (!taskResult.successful)
                {
                    Debug.Log("Task failed");
                    yield break;
                }

                // Get the audio data from the response
                byte[] audioBytes = Convert.FromBase64String(taskResult.value);

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

                student.GetComponent<SmartStudentController>().AddQuestion(audioClip);

                yield break;
            }   
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

    //Remove this garbage if not used later
    private Tuple<DateTime, AudioClip> GetFreshTuple(Queue<Tuple<DateTime, AudioClip>> queue, double maxMinutes = 2)
    {
        if (queue.Count == 0) return null;
    
        Tuple<DateTime, AudioClip> tuple = null;

        do
        {
            try
            {
                tuple = queue.Dequeue();
            }
            catch (System.Exception)
            {
                return null;
            }

        } while (DateTime.Now - tuple.Item1 > TimeSpan.FromMinutes(maxMinutes));    
    
        return tuple;
    }
}

