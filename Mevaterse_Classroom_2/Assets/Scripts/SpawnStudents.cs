using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnStudents : MonoBehaviourPunCallbacks
{
    private GameObject chairs;
    private int chairNumber;
    private bool[] assignedSeats;
    private int studentNumber;
    void Start()
    {
        chairs = GameObject.Find("chairs");

        chairNumber = chairs.transform.childCount;
        assignedSeats = new bool[chairNumber];
        
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        InstantiateStudent(studentNumber);
    }

    public void InstantiateStudent(int studentNumber)
    {
        if (chairs == null)
        {
            throw new System.Exception("Chairs GameObject is null!");
        }

        if (chairNumber == 0)
        {
            throw new System.Exception("Number of chairs is 0!");
        }

        if(studentNumber <= 0)
        {
            return;
        } 
        
        else if(studentNumber > chairNumber)
        {
            Debug.LogError("Not enough chairs for all students! \nSetting maximum number of students to number of chairs.");
            studentNumber = chairNumber;
        }

        int randomIndex;

        for (int i = 0; i < studentNumber; i++)
        {

            do
            {
                randomIndex = Random.Range(0, chairNumber);
                
            } while (assignedSeats[randomIndex]);

            assignedSeats[randomIndex] = true;
            

            Transform randomChair = chairs.transform.GetChild(randomIndex);

            if (randomChair == null)
            {
                Debug.LogError("Chair transform is null!");
                return;
            }

            Vector3 spawnPosition = randomChair.position + new Vector3(0, .6f, .1f);
            GameObject student = PhotonNetwork.Instantiate("Student", spawnPosition, Quaternion.identity);
            
            if (student == null)
            {
                throw new System.Exception("Failed to instantiate SmartStudent!");
            }
            
            Animator studentAnimator = student.GetComponent<Animator>();
            studentAnimator.Play("Idle");

            student.transform.parent = randomChair;
       }
    }

    public void SetStudentNumber(int number)
    {
        studentNumber = number;
    }

}
