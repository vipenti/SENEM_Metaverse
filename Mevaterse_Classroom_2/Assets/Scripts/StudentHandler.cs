using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;


public class StudentHandler : MonoBehaviourPunCallbacks
{
    private GameObject[] students;
    private Queue<SmartStudentController> questionsQueue = new Queue<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        if (questionsQueue.Count > 0)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                
            }
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        StartCoroutine(GetStudents());
    }

    private IEnumerator GetStudents()
    {
        yield return new WaitForSeconds(1);
        students = GameObject.FindGameObjectsWithTag("SmartStudent");
    }

    private GameObject GetRandomStudent()
    {
        if (students.Length <= 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, students.Length);
        return students[randomIndex];
    }

    public void AddQuestion(AudioClip clip)
    {
        GameObject student = GetRandomStudent();
        if (student == null)
        {
            Debug.LogError("No students found!");
            return;
        }

        SmartStudentController studentController = student.GetComponent<SmartStudentController>();
        studentController.AddQuestion(clip);

        if(!questionsQueue.Contains(studentController))
        {
            questionsQueue.Enqueue(studentController);
        }
    }

    private void PlayQuestion()
    {
        if (questionsQueue.Count <= 0)
        {
            return;
        }

        SmartStudentController studentController = questionsQueue.Dequeue();
        studentController.PlayQuestion();
    }


}
