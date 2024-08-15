using UnityEngine;
using System;
using System.Collections;

// Class to handle the smart student behaviour
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

        // Subscribe to the event
        addedQuestion += RaiseHand; 
    }

    // Update the model to raise the hand
    void RaiseHand(object sender, EventArgs e){
        
        if(!isHandRaised)
        {
            Debug.Log("Updating model...");

            animatorController.SetBool("HandRaised", true);
            isHandRaised = true;
        }
   }

    // Add a question to the student
    public void AddQuestion(AudioClip clip)
    {
        if (clip == null) return;

        question.clip = clip;
        OnAddedQuestion();
    }

    // Event that fires when a question is added
    private void OnAddedQuestion()
    {
        addedQuestion?.Invoke(this, EventArgs.Empty);
    }

    // Play the question and update the model
    public void PlayQuestion()
    {
        question.Play();
        animatorController.SetBool("HandRaised", false);
        animatorController.SetBool("Talking", true);

        isHandRaised = false;
        isTalking = true;

        // stop the talking animation after the clip ends
        StartCoroutine(StopTalkingAnimation(question.clip.length));

        // Clear the question after it's played
        question.clip = null;
    }

    private IEnumerator StopTalkingAnimation(float clipLength)
    {
        // wait for the clip to end then stop the talking animation
        yield return new WaitForSeconds(clipLength);

        if(!isTalking)
        {
            animatorController.SetBool("Talking", false);
            isTalking = false;
        }
    }

}
