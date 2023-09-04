using Vipenti.Singletons;
using System;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class LogManager : Singleton<LogManager>
{
    string filePath;
    string whiteboardPath;
    private void Start()
    {
        //if (!PhotonNetwork.IsMasterClient) return;
        filePath = $"{Application.dataPath}/log{DateTime.UtcNow.Date.ToString("MM-dd-yyyy")}_{DateTime.Now.ToString("HH.mm.ss")}.txt";
        whiteboardPath = $"{Application.dataPath}/Whiteboard{DateTime.UtcNow.Date.ToString("MM-dd-yyyy")}_{DateTime.Now.ToString("HH.mm.ss")}.txt";
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
            Debug.Log("Created file in " + filePath);
        }
        if (!File.Exists(whiteboardPath))
        {
            File.Create(whiteboardPath);
            Debug.Log("Created file in " + whiteboardPath);
        }
    }

    // Update is called once per frame
    public void LogInfo(string msg)
    {
        //if (!PhotonNetwork.IsMasterClient) return;
        string text = $"{DateTime.Now.ToString("HH:mm:ss")}: {msg}";
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(text);
        }
    }

    public void SaveNotes(string msg)
    {
        string text = $"{msg}";
        //File.WriteAllText(notesPath, text);
    }

    public void LogWhiteboard(string msg)
    {
        using (StreamWriter writer = File.AppendText(whiteboardPath))
        {
            writer.WriteLine(msg);
        }
    }
}
