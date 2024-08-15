using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Class to spawn students in the room on creation
public class SpawnStudents : MonoBehaviourPunCallbacks
{
    private GameObject chairs; // GameObject containing all the chairs in the room
    private int chairNumber;
    private bool[] assignedSeats; // Array to keep track of assigned seats, element true if seat is taken
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

        // For each student, assign a random chair
        for (int i = 0; i < studentNumber; i++)
        {
            // Find a random chair that is not taken
            do
            {
                randomIndex = Random.Range(0, chairNumber);
                
            } while (assignedSeats[randomIndex]);

            // Mark the chair as taken
            assignedSeats[randomIndex] = true;
            
            // Get the chair transform
            Transform randomChair = chairs.transform.GetChild(randomIndex);

            if (randomChair == null)
            {
                Debug.LogError("Chair transform is null!");
                return;
            }

            // Instantiate the student and position it on the chair
            Vector3 spawnPosition = randomChair.position + new Vector3(0, .6f, .1f);
            GameObject student = PhotonNetwork.Instantiate("Student", spawnPosition, Quaternion.identity);
            
            if (student == null)
            {
                throw new System.Exception("Failed to instantiate SmartStudent!");
            }
            
            // Start the student's idle animation in case it's not already playing
            Animator studentAnimator = student.GetComponent<Animator>();
            studentAnimator.Play("Idle");

            // Set the student's parent to the chair
            student.transform.parent = randomChair;
       }
    }

    // Public method called on ConnectToServer.cs to set the number of students
    public void SetStudentNumber(int number)
    {
        studentNumber = number;
    }

}
