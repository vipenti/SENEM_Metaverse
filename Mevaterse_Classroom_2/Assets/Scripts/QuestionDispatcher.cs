using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

// Class to handle the communication between Unity and the server
public class QuestionDispatcher : MonoBehaviour
{ 
    private GameObject student;
    private StudentHandler studentHandler;
    private int startingWaitingTime = 5, // Time to wait before starting to check for the result
                waitingTime = 5; // Time to wait between each check

    private const int maxRetries = 10; // Maximum number of retries on server request

    // Serialisable classes for JSON parsing
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
        public string value;
    }

    public delegate IEnumerator ResponseCallback(string response);

    void Start()
    {
        studentHandler = GameObject.Find("StudentHandler").GetComponent<StudentHandler>();
    }

    // Initialize the student model, called on ConnectToServer.cs while creating the room
    public void StartStudent(string text)
    {
        // Initialize the student model on the server giving it the topic of the "lesson"
        StartCoroutine(SendTextToServer(text));
    }

    // Send an audio clip to the server
    public void AddAudioClip(AudioClip clip, DateTime? date = null)
    {
        // if no date is provided, use the current date
        if (date == null) date = DateTime.Now;

        StartCoroutine(SendAudioToServer(new Tuple<DateTime, AudioClip>((DateTime)date, clip), GetAudioFromServer));
        // StartCoroutine(SendAudioToServer(new Tuple<DateTime, AudioClip>((DateTime)date, clip), "http://127.0.0.1:5000/test_stub"));
    }

    private IEnumerator SendTextToServer(string text, string url = "http://127.0.0.1:5000/start")
    {  
        UnityWebRequest www = new UnityWebRequest();
        Debug.Log("Sending text...");

        // Create a new TextData object and convert it to a JSON string
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

    // Send an audio clip to the server and get task id to check for the result
    private IEnumerator SendAudioToServer(Tuple<DateTime, AudioClip> clip, ResponseCallback callback, string url = "http://127.0.0.1:5000/generate_question")
    {
        UnityWebRequest www = new UnityWebRequest();
        var data = new TaskID();

        // if tuple and its audio clip are not null
        if (clip != null && clip.Item2 != null)
        {
            // Convert the audio clip to a byte array
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
                // Get the task id from the response, so that once the task is done, we can get the audio
                Debug.Log("ID arrived!");

                data = JsonUtility.FromJson<TaskID>(www.downloadHandler.text);

                string id = data.result_id;

                Debug.Log("ID: " + id);

                // Wait for a few seconds before checking for the result
                yield return new WaitForSeconds(startingWaitingTime);

                // Start the coroutine to get the audio from the server
                StartCoroutine(callback?.Invoke("http://localhost:5000/result/" + id));    
            }

            
        }

        www.Dispose();
    }

    // Get the audio from the server using the task id
    private IEnumerator GetAudioFromServer(string url = "http://localhost:5000/result/0"){

        // number of request retries
        int retries = 0;

        while(true){
            Debug.Log("Checking for audio...");
            Debug.Log("URL: " + url);

            UnityWebRequest www2 = new UnityWebRequest(url, "POST")
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

                Debug.Log("Response arrived!");
                Debug.Log("Response: " + www2.downloadHandler);

                TaskResult taskResult = JsonUtility.FromJson<TaskResult>(www2.downloadHandler.text);

                // if the task is not ready yet, wait for a few seconds and try again
                if (!taskResult.ready && retries < maxRetries)
                {
                    Debug.Log("Task not ready yet, trying again in " + waitingTime + " seconds...");
                    retries++;

                    yield return new WaitForSeconds(waitingTime);
                    continue;
                }

                //if the task failed, stop the coroutine
                if (!taskResult.successful)
                {
                    Debug.Log("Task failed");
                    yield break;
                }

                Debug.Log("Audio recording arrived!");

                // Get the audio data from the response
                byte[] audioBytes = Convert.FromBase64String(taskResult.value);

                // Convert the byte array to a float array
                float[] audioDataResponse = new float[audioBytes.Length / 2];
                
                // Turn into correct format
                for (int i = 0; i < audioBytes.Length; i += 2)
                {
                    short sample = BitConverter.ToInt16(audioBytes, i);
                    audioDataResponse[i / 2] = sample / 32768.0f;
                }

                // Create a new AudioClip and set the audio data
                AudioClip audioClip = AudioClip.Create("ReceivedAudio", audioDataResponse.Length, 1, 24000, false);
                audioClip.SetData(audioDataResponse, 0);

                // Add the audio clip to the student model
                // student.GetComponent<SmartStudentController>().AddQuestion(audioClip);
                studentHandler.AddQuestion(audioClip);
            }

            www2.Dispose();
            yield break;   
        }
    }

    // Get the text from the server using the task id
    private IEnumerator GetTextFromServer(string url = "http://localhost:5000/result/0"){
        Debug.Log("Checking for audio...");
        Debug.Log("URL: " + url);

        UnityWebRequest www2 = new UnityWebRequest(url, "POST")
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

            Debug.Log("Response arrived!");
            Debug.Log("Response: " + www2.downloadHandler);

            TaskResult taskResult = JsonUtility.FromJson<TaskResult>(www2.downloadHandler.text);

            //if the task failed, stop the coroutine
            if (!taskResult.successful)
            {
                Debug.Log("Task failed");
                yield break;
            }

            Debug.Log("Text question arrived!");
        }

        www2.Dispose();

        // TODO: Add chatbot response to the student model
    }

    // Convert an audio clip to a byte array in WAV format
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

}

