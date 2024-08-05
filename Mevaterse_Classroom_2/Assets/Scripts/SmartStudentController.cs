using UnityEngine;
using System;

public class SmartStudentController : MonoBehaviour
{
    private AudioSource question;
    private QuestionDispatcher questionDispatcher;
    public event EventHandler addedQuestion;
    private Animator animatorController;
    private bool isTalking;
    private bool isHandRaised;


    void Start()
    {   
        question = gameObject.AddComponent<AudioSource>();
        questionDispatcher = GameObject.Find("QuestionDispatcher").GetComponent<QuestionDispatcher>();
        animatorController = GetComponent<Animator>();

        isHandRaised = false;
        isTalking = false;

        addedQuestion += RaiseHand; 
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            question.Play();
            animatorController.SetBool("doesStudentRaiseHand", false);
            isHandRaised = false;
            isTalking = true;

            question.clip = null;
        }
    }

    void RaiseHand(object sender, EventArgs e){
        // Update the model
        if(!isHandRaised)
        {
            Debug.Log("Updating model...");

            animatorController.SetBool("doesStudentRaiseHand", true);
            isHandRaised = true;
        }
   }

    public void AddQuestion(AudioClip clip)
    {
        if (clip == null) return;

        question.clip = clip;
        OnAddedQuestion();
    }

    void OnAddedQuestion()
    {
        addedQuestion?.Invoke(this, EventArgs.Empty);
    }

}
