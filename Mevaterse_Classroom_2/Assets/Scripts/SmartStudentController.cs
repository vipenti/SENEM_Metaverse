using UnityEngine;
using System;

public class SmartStudentController : MonoBehaviour
{
    public AudioSource question;
    public event EventHandler addedQuestion;
    private Animator animatorController;
    private bool isTalking;
    private bool isHandRaised;


    void Start()
    {   
        question = gameObject.AddComponent<AudioSource>();
        animatorController = GetComponent<Animator>();

        isHandRaised = false;
        isTalking = false;

        addedQuestion += RaiseHand; 
    }

    void RaiseHand(object sender, EventArgs e){
        // Update the model
        if(!isHandRaised)
        {
            Debug.Log("Updating model...");

            animatorController.SetBool("HandRaised", true);
            isHandRaised = true;
        }
   }

    public void AddQuestion(AudioClip clip)
    {
        if (clip == null) return;

        question.clip = clip;
        OnAddedQuestion();
    }

    private void OnAddedQuestion()
    {
        addedQuestion?.Invoke(this, EventArgs.Empty);
    }

    public void PlayQuestion()
    {
        question.Play();
        animatorController.SetBool("HandRaised", false);
        isHandRaised = false;
        isTalking = true;

        question.clip = null;
    }

}
