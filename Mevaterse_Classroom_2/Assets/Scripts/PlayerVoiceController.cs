using System;
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
        outputSource;
    public bool isTalking;
    private bool isTyping;
    private QuestionDispatcher questionDispatcher;

    private void Start()
    {
        outputSource = gameObject.AddComponent<AudioSource>();
        questionDispatcher = GameObject.Find("QuestionDispatcher").GetComponent<QuestionDispatcher>();

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

    public void Update()
    {
        if (!view.IsMine) return;

        isTyping = gameObject.GetComponent<PlayerController>().isTyping;

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

            if(!Microphone.IsRecording(null)){
                outputSource.clip = Microphone.Start(null, false, 40, 44100);

            }            
        }
        else
        {
            isTalking = false;
            info.text = "";

            if(Microphone.IsRecording(null)){
                int maxRecordingTime = 3;

                // Capture the current clip data
                int position = Microphone.GetPosition(null);
                Debug.Log("--POSITION: " + position);

                Microphone.End(null);

                if (outputSource.clip == null) return;

                

                float[] soundData = new float[outputSource.clip.samples * outputSource.clip.channels];
                outputSource.clip.GetData (soundData, 0);

                // Create shortened array for the data used for recording
                float[] newData = new float[position * outputSource.clip.channels];

                // Copy the used samples to a new array
                for (int i = 0; i < newData.Length; i++) {
                    newData[i] = soundData[i];
                }

                // Make new AudioClip with the correct length
                AudioClip newClip = AudioClip.Create (outputSource.clip.name,
                                position,
                                outputSource.clip.channels,
                                outputSource.clip.frequency,
                                false);

                newClip.SetData (newData, 0); // Give it the data from the old clip

                // Replace the old clip
                AudioClip.Destroy (outputSource.clip);
                outputSource.clip = newClip;

                Debug.Log("--LENGHT: " + outputSource.clip.length);

                if(outputSource.clip == null){
                    throw new Exception("!!Transformed audio clip is null");
                }

                if (outputSource.clip.length > maxRecordingTime)
                {
                   questionDispatcher.AddAudioClip(outputSource.clip, DateTime.Now);
                }

                else if (outputSource.clip.length <= maxRecordingTime)
                {
                    Debug.Log("Audio clip is too short");
                }

                outputSource.clip = null;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!photonView.IsMine) return;
        view.RPC("ToggleMicRpc", RpcTarget.All, speaker.enabled);
    }

    [PunRPC]
    public void ToggleMicRpc(bool value)
    {
        speaker.enabled = value;
        audioSource.enabled = value;
    }

}


