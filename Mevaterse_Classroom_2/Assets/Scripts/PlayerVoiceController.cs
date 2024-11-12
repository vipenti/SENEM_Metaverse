using System;
using System.Threading;
using Photon.Voice.Unity;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerVoiceController : MonoBehaviourPunCallbacks
{
    readonly string muteMsg = "<sprite index=2> \n     You are muted";
    readonly string unmuteMsg = "<sprite index=1> \n     Your mic is on!";

    private Recorder recorder;
    private TMP_Text info;
    private TMP_Text microphoneIndicator;
    private TMP_Text microphoneInfo;
    private PhotonView view;
    private TextChat textChat;

    public Speaker speaker;
    public AudioSource audioSource,
                    outputSource; // Audio source for the output audio
    public bool isTalking;
    private bool isTyping;

    private QuestionDispatcher questionDispatcher;
    private const float leniencyPeriod = 3.0f;
    private const int maxRecordingTime = 40;
    private const int minRecordingTime = 3;
    private float silenceTimer = 0.0f;

    private void Start()
    {
        outputSource = gameObject.AddComponent<AudioSource>();
        recorder = GameObject.Find("VoiceManager").GetComponent<Recorder>();
        info = GameObject.Find("SoundState").GetComponent<TMP_Text>();
        microphoneIndicator = GameObject.Find("MicState").GetComponent<TMP_Text>();
        microphoneInfo = GameObject.Find("MicInfo").GetComponent<TMP_Text>();

        view = GetComponent<PhotonView>();
        textChat = GameObject.Find("TextChat").GetComponent<TextChat>();

        info.text = "";
        isTalking = false;
        isTyping = false;
        speaker.enabled = false;

        if (photonView.IsMine)
        {
            microphoneIndicator.text = muteMsg;
            if (Microphone.devices.Length > 0)
                microphoneInfo.text = $"Using: {Microphone.devices[0]}";
        }
    }

    private void Update()
    {
        if (!view.IsMine) return;

        if (Input.GetKeyUp(KeyCode.Tab) && !speaker.enabled && !textChat.isSelected && !isTyping)
        {
            view.RPC("ToggleMicRpc", RpcTarget.All, true);
            microphoneIndicator.text = unmuteMsg;
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && speaker.enabled && !textChat.isSelected && !isTyping)
        {
            view.RPC("ToggleMicRpc", RpcTarget.All, false);
            microphoneIndicator.text = muteMsg;
        }

        if (speaker.enabled && recorder.LevelMeter.CurrentAvgAmp > 0.01f)
        {
            isTalking = true;
            info.text = "<color=\"green\">Transmitting audio</color>";
            if (Microphone.IsRecording(null))
            {
                silenceTimer = 0.0f;
            }
            else
            {
                Debug.Log("Starting recording");
                outputSource.clip = Microphone.Start(null, false, maxRecordingTime, 44100);
            }
        }
        else
        {
            isTalking = false;
            info.text = "";

            if (Microphone.IsRecording(null) && (Presenter.Instance.presenterID == PhotonNetwork.LocalPlayer.UserId))
            {
                silenceTimer += Time.deltaTime;
            }
        }

        if (silenceTimer >= leniencyPeriod && (Presenter.Instance.presenterID == PhotonNetwork.LocalPlayer.UserId))
        {
            StopRecording();
            silenceTimer = 0.0f;
        }
    }

    private void StopRecording()
    {
        Debug.Log("Stopping recording");

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected.");
            return;
        }

        int position = Microphone.GetPosition(null);
        Microphone.End(null);

        if (outputSource == null || outputSource.clip == null || position <= 0)
        {
            Debug.LogWarning("No audio data recorded.");
            return;
        }

        CorrectAudioClipLength(outputSource, position);

        if (outputSource.clip == null || outputSource.clip.length <= minRecordingTime)
        {
            Debug.Log("Audio clip is too short");
            return;
        }

        NotifyStudents(outputSource.clip); // Notifica agli studenti che la clip è pronta

        AudioClip.Destroy(outputSource.clip);
    }

    private void CorrectAudioClipLength(AudioSource audioClip, int position)
    {
        float[] soundData = new float[audioClip.clip.samples * audioClip.clip.channels];
        audioClip.clip.GetData(soundData, 0);

        float[] newData = new float[position * audioClip.clip.channels];
        Array.Copy(soundData, newData, newData.Length);

        AudioClip newClip = AudioClip.Create(audioClip.clip.name, position, audioClip.clip.channels, audioClip.clip.frequency, false);
        newClip.SetData(newData, 0);
        AudioClip.Destroy(audioClip.clip);
        audioClip.clip = newClip;
    }

    private void NotifyStudents(AudioClip audioClip)
    {
        SmartStudentController[] students = FindObjectsOfType<SmartStudentController>();
        foreach (var student in students)
        {
            student.EvaluateAudioForQuestion(audioClip); // Ogni studente fa il suo "roll" per inviare la richiesta
        }
    }

    [PunRPC]
    public void ToggleMicRpc(bool value)
    {
        speaker.enabled = value;
        audioSource.enabled = value;
    }
}
