using System.Linq;
using Vipenti.Singletons;
using TMPro;
using UnityEngine;
using System;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    private TextMeshProUGUI debugAreaText = null;

    [SerializeField]
    private bool enableDebug = true;

    private int currentLines = 0;

    void Awake()
    {
        if (debugAreaText == null)
        {
            debugAreaText = GetComponent<TextMeshProUGUI>();
        }
        debugAreaText.text = string.Empty;

    }

    void OnEnable()
    {        
        debugAreaText.enabled = true;
        enabled = true;

        if (enabled)
        {
            Debug.Log("Logger enabled " + debugAreaText == null);
            debugAreaText.text += $"<color=\"white\">{DateTime.Now.ToString("HH:mm:ss.fff")} {this.GetType().Name} enabled</color>\n";
        }
    }

    public void LogInfo(string message)
    {
        ClearLines();
        currentLines++;
        debugAreaText.text += $"<color=\"green\">{DateTime.Now.ToString("HH:mm:ss")}</color> <color=\"white\">{message}</color>\n";
    }

    public void LogError(string message)
    {
        ClearLines();
        currentLines++;
        debugAreaText.text += $"<color=\"red\">{DateTime.Now.ToString("HH:mm:ss")} {message}</color>\n";
    }

    public void LogWarning(string message)
    {
        ClearLines();
        currentLines++;
        debugAreaText.text += $"<color=\"yellow\">{DateTime.Now.ToString("HH:mm:ss")} {message}</color>\n";
    }

    private void ClearLines()
    {
        if (debugAreaText.text.Length >= 320 || currentLines >= 10)
        {
            debugAreaText.text = "";
            currentLines = 0;
        }
    }
}