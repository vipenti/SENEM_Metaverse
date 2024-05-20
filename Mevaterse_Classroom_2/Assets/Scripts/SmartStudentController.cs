using UnityEngine;
using System;

public class SmartStudentController : MonoBehaviour
{
    private AudioSource question;
    private QuestionDispatcher questionDispatcher;
    public event EventHandler addedQuestion;
    void Start()
    {   
        question = GetComponent<AudioSource>();
        questionDispatcher = GameObject.Find("QuestionDispatcher").GetComponent<QuestionDispatcher>();

        addedQuestion += RaiseHand; 
    }

    void Update()
    {
        if(!question.isPlaying)
        {
            RetrieveQuestion();
        }
    }

    void RetrieveQuestion()
    {
        AudioClip clip = questionDispatcher.GetQuestion();

        if (clip == null) return;

        question.clip = clip;
        addedQuestion?.Invoke(this, EventArgs.Empty);
    }

    void RaiseHand(object sender, EventArgs e){
        // Update the model
        Debug.Log("Updating model...");
    }
}
