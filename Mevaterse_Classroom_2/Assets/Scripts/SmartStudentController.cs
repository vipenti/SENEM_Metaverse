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
    private GameObject volumeIcon;


    void Start()
    {   
        volumeIcon = transform.Find("PlayerOverhead").Find("VolumeIcon").gameObject;
        question = gameObject.AddComponent<AudioSource>();
        animatorController = GetComponent<Animator>();

        isHandRaised = false;
        isTalking = false;

        volumeIcon.SetActive(false);

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
        animatorController.SetBool("IsTalking", true);

        isHandRaised = false;
        isTalking = true;

        volumeIcon.SetActive(true);

        // stop the talking animation after the clip ends
        StartCoroutine(StopTalkingAnimation(question.clip.length));
    }

    private IEnumerator StopTalkingAnimation(float clipLength)
    {
        // wait for the clip to end then stop the talking animation
        yield return new WaitForSeconds(clipLength);

        volumeIcon.SetActive(false);

        if(!isTalking)
        {
            animatorController.SetBool("IsTalking", false);
            isTalking = false;
        }
    }

}
