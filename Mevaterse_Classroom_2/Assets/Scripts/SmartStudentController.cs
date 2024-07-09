using UnityEngine;
using System;

public class SmartStudentController : MonoBehaviour
{
    private AudioSource question;
    private QuestionDispatcher questionDispatcher;
    public event EventHandler addedQuestion;
    private Renderer rend;

    void Start()
    {   
        question = GetComponent<AudioSource>();
        questionDispatcher = GameObject.Find("QuestionDispatcher").GetComponent<QuestionDispatcher>();
        rend = GetComponent<Renderer>();

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
            rend.material.color = Color.red;
        }
    }

    void RaiseHand(object sender, EventArgs e){
        // Update the model
        Debug.Log("Updating model...");

        rend.material.color = UnityEngine.Random.ColorHSV();
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
