using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using System.Collections.Generic;

public class SmartStudentController : MonoBehaviourPun
{
    public AudioSource question;
    private Animator animatorController;
    private bool isTalking;
    private bool isHandRaised;
    private GameObject volumeIcon;
    private PhotonView textChatView;

    // Tratti di personalit� per ogni studente
    public Personality personality;
    public Intelligence intelligence;
    public Interest interest;
    public Happiness happiness;

    private float questionTriggerProbability;
    private QuestionDispatcher questionDispatcher;
    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
    private Queue<string> stringQueue = new Queue<string>();
    private TMP_Text overhead;
    private TMP_Text rollDisplay; // Aggiunto per visualizzare il roll
    private TextChat textChat;


    void Start()
    {
        textChat = GameObject.Find("TextChat").GetComponent<TextChat>();
        volumeIcon = transform.Find("PlayerOverhead").Find("VolumeIcon").gameObject;
        question = gameObject.AddComponent<AudioSource>();
        animatorController = GetComponent<Animator>();
        questionDispatcher = GameObject.Find("QuestionDispatcher").GetComponent<QuestionDispatcher>();

        isHandRaised = false;
        isTalking = false;
        volumeIcon.SetActive(false);

        questionTriggerProbability = CalculateQuestionProbability();
        overhead = transform.Find("PlayerOverhead").Find("PlayerName").gameObject.GetComponent<TMP_Text>();
        overhead.text = $"{personality} Student, {questionTriggerProbability * 100}%";

        // Display roll value
        rollDisplay = transform.Find("PlayerOverhead").Find("RollDisplay").GetComponent<TMP_Text>();

    }

    void LateUpdate()
    {
        animatorController.SetBool("HandRaised", isHandRaised);
    }

    private float CalculateQuestionProbability()
    {
        float baseProbability = 0.1f;
        baseProbability += (float)interest / 10f; // Aumenta in base all'interesse
        baseProbability += (float)personality / 20f; // Aumenta in base alla personalit�
        return Mathf.Clamp01(baseProbability); // Limita tra 0 e 1
    }

    public bool Roll()
    {
        float roll = UnityEngine.Random.value;
        rollDisplay.text = $"Roll: {roll:F2}"; // Mostra il roll nell'overhead
        return (roll <= questionTriggerProbability);
    }

    public void EvaluateAudioForQuestion(AudioClip audioClip)
    {
        if (Roll())
        {
            questionDispatcher.AddQuestionRequest(audioClip, this, personality, intelligence, interest, happiness);
        }
    }

    public void EnqueueAudioResponse(AudioClip responseAudio)
    {
        audioQueue.Enqueue(responseAudio);
        RaiseHand();
    }

    public void EnqueueTextResponse(string responseText)
    {
        stringQueue.Enqueue(responseText);
    }

    // Metodo modificato per riprodurre solo quando chiamato da StudentHandler
    public void TriggerPlayNextAudio()
    {
        PlayNextAudio();
        SendNextQuestion();
    }

    private void PlayNextAudio()
    {
        if (!question.isPlaying && audioQueue.Count > 0)
        {
            question.clip = audioQueue.Dequeue();
            question.Play();
            animatorController.SetBool("IsTalking", true);
            volumeIcon.SetActive(true);
            isHandRaised = false;
            StartCoroutine(StopTalkingAnimation(question.clip.length));
        }
    }

    private void SendNextQuestion()
    {
        if (!question.isPlaying && stringQueue.Count > 0)
        {
            string questionText = stringQueue.Dequeue();
            photonView.RPC("WriteQuestionInChat", RpcTarget.All, questionText);
        }
    }

    private IEnumerator StopTalkingAnimation(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        volumeIcon.SetActive(false);
        isTalking = false;
        volumeIcon.SetActive(false);
        animatorController.SetBool("IsTalking", false);

        if (audioQueue.Count > 0)
        {
            PlayNextAudio();
        }
        else if (photonView.IsMine)
        {
            photonView.RPC("PlayAnimation", RpcTarget.All, "Idle");
        }
    }

    void RaiseHand()
    {
        if (photonView.IsMine && !isHandRaised)
        {
            isHandRaised = true;
            photonView.RPC("PlayAnimation", RpcTarget.All, "Hand Raise");
            photonView.RPC("NotifyHandRaised", RpcTarget.All); // Cambiato per corrispondere al metodo esistente
        }
    }


    [PunRPC]
    public void PlayAnimation(string animationName)
    {
        animatorController.Play(animationName);
    }

    [PunRPC]
    public void NotifyHandRaised()
    {
        string msg = $"<color=\"yellow\">{personality} Student </color>: Professor!";
        Logger.Instance.LogInfo(msg);
        LogManager.Instance.LogInfo(msg);
    }

    [PunRPC]
    public void WriteQuestionInChat(string question)
    {
        string msg = $"<color=\"yellow\">{personality} Student </color>: {question}";
        Logger.Instance.LogInfo(msg);
        LogManager.Instance.LogInfo(msg); ;
    } 
}
