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
    private bool isTimerActivable; // flag to allow leniency timer to start
    private volatile bool stopRecordingFlag; // Flag to stop recording

    private QuestionDispatcher questionDispatcher;

    private Timer leniencyTimer, // Timer for alllowing brief pauses of "leniencyPeriod" in speech
        recordingTimer;         // Timer stopping recording after "maxRecordingTime" seconds
    private const int leniencyPeriod = 1000; // 1 second
    private const int maxRecordingTime = 40; // 40 seconds
    private const int minRecordingTime = 3; // 3 seconds

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

        isTimerActivable = false;
        stopRecordingFlag = false;

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

            // If the microphone is already recording suspend the leniency timer
            // and allow leniency timer to start in future in case of a pause
            if(Microphone.IsRecording(null))
            {
                isTimerActivable = true;

                SuspendTimer(leniencyTimer);
            }

            // If the microphone is not recording start recording
            // and start a timer to end recording after maxRecordingTime seconds
            else
            {
                isTimerActivable = true;

                Debug.Log("Starting recording");
                outputSource.clip = Microphone.Start(null, false, maxRecordingTime + 1, 44100);
                StartOrResetTimer(recordingTimer, (maxRecordingTime) * 1000, SetStopRecordingFlag); //after maxRecordingTime seconds stop recording
            }            
        }

        // If the microphone is not detecting any audio
        else
        {
            isTalking = false;
            info.text = "";

            // If the microphone is recording and there is a pause in speech start the leniency timer
            if(Microphone.IsRecording(null) && isTimerActivable){
                isTimerActivable = false;
                Debug.Log("Starting leniency timer");

                StartOrResetTimer(leniencyTimer, leniencyPeriod, SetStopRecordingFlag); // after leniencyPeriod seconds of silence stop recording
            }
        }

        // If the stopRecordingFlag is set to true stop recording
        if(stopRecordingFlag){
            stopRecordingFlag = false;
            StopRecording();
        }
    }

    // Correct the length of the audio clip to the actual length of the recording since Unity does not support dynamic length audio clips
    private void CorrectAudioClipLength(AudioSource audioClip, int position)
    {
        Debug.Log(audioClip);

        float[] soundData = new float[audioClip.clip.samples * audioClip.clip.channels];
        audioClip.clip.GetData (soundData, 0);

        // Create shortened array for the data used for recording
        float[] newData = new float[position * audioClip.clip.channels];

        // Copy the used samples to a new array
        for (int i = 0; i < newData.Length; i++) {
            newData[i] = soundData[i];
        }

        // Make new AudioClip with the correct length
        AudioClip newClip = AudioClip.Create (audioClip.clip.name,
                        position,
                        audioClip.clip.channels,
                        audioClip.clip.frequency,
                        false);

        newClip.SetData (newData, 0); // Give it the data from the old clip

        // Replace the old clip
        AudioClip.Destroy(audioClip.clip);
        audioClip.clip = newClip;
    }

    // Stop recording and send the audio clip to the QuestionDispatcher
    private void StopRecording(){
        Debug.Log("Stopping recording");

        // Suspend the timers to prevent them from stopping the already stopped recording
        SuspendTimer(recordingTimer);
        SuspendTimer(leniencyTimer);

        // Capture the current clip position in terms of samples recorded
        int position = Microphone.GetPosition(null);

        // Stop recording
        Microphone.End(null);

        // If the audio clip is null return
        if (outputSource == null)
        {
            Debug.Log("Audio clip is null");
            return;
        }

        CorrectAudioClipLength(outputSource, position);

        Debug.Log("--LENGTH: " + outputSource.clip.length);
        
        // if for some alien reason the audio clip is still null throw an exception
        if(outputSource.clip == null){
            throw new Exception("!!Transformed audio clip is null");
        }
        
        // If the audio clip is longer than minRecordingTime seconds send it to the QuestionDispatcher
        if (outputSource.clip.length > minRecordingTime)
        {
            questionDispatcher.AddAudioClip(outputSource.clip, DateTime.Now);
        }

        // If the audio clip is shorter than minRecordingTime seconds ignore it
        else if (outputSource.clip.length <= minRecordingTime)
        {
            Debug.Log("Audio clip is too short");
        }

        // Dispose of the audio clip
        AudioClip.Destroy(outputSource.clip);
    }

    // Starts or resets a timer with its time and callback function
    private void StartOrResetTimer(Timer timer = null, int time = 1000, Action callbackFunction = null)
    {
        // If the callback function is null throw an exception
        if(callbackFunction == null)
        {
            throw new ArgumentNullException(nameof(callbackFunction), "  -Callback function in StartOrResetTimer cannot be null");
        }

        // Convert Action to TimerCallback with a lambda expression
        TimerCallback timerCallback = state => callbackFunction();

        // If the timer is null create a new timer
        if (timer == null)
        {
            timer = new Timer(timerCallback, null, time, Timeout.Infinite);
        }

        else
        {
            // Reset the timer
            timer.Change(time, Timeout.Infinite);
        }
    }

    // Suspends a timer
    private void SuspendTimer(Timer timer = null)
    {
        timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    // Sets the stopRecordingFlag to true
    private void SetStopRecordingFlag(){
        stopRecordingFlag = true;
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


