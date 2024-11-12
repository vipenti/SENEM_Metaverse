using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StudentHandler : MonoBehaviourPunCallbacks
{
    private Queue<SmartStudentController> studentsQueue;
    private Queue<SmartStudentController> textsQueue;// Coda per gestire gli studenti che hanno una domanda
    private GameObject[] students; // Array di tutti gli studenti nella stanza

    void Start()
    {
        studentsQueue = new Queue<SmartStudentController>();
        textsQueue = new Queue<SmartStudentController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (Presenter.Instance.presenterID == PhotonNetwork.LocalPlayer.UserId))
        {
            PlayQuestion();
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        StartCoroutine(GetStudents());
    }

    private IEnumerator GetStudents(int waitTime = 1)
    {
        yield return new WaitForSeconds(waitTime);
        students = GameObject.FindGameObjectsWithTag("SmartStudent");
        Debug.Log($"Created {students.Length} students");
    }

    // Aggiunge l'audio alla coda dello studente specifico
    public void AddAudioToQueue(AudioClip audioClip, SmartStudentController studentController)
    {
        studentController.EnqueueAudioResponse(audioClip);
        if (!studentsQueue.Contains(studentController))
        {
            studentsQueue.Enqueue(studentController);
        }
    }

    public void AddTextToQueue(string text, SmartStudentController studentController)
    {
        studentController.EnqueueTextResponse(text);
        if (!textsQueue.Contains(studentController))
        {
            textsQueue.Enqueue(studentController);
        }
    }

    // Riproduce la domanda (audio) del primo studente nella coda
    private void PlayQuestion()
    {
        if (studentsQueue.Count <= 0)
        {
            Debug.Log("Nessuna domanda in coda.");
            return;
        }

        SmartStudentController studentController = studentsQueue.Dequeue();
        SmartStudentController textStudentController = textsQueue.Dequeue();
        studentController.TriggerPlayNextAudio(); 
    }
}
