using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animatorController;
    public Light light;
    private Color32 lightColor;
    private Color32 lightColorParty;
    private float elapsedTime = 0;
    private bool party;
    public AudioSource audioSource;

    void Start()
    {
        lightColor = light.color;
        party = false;
        lightColorParty = new Color32(0, 0, 200, 255);
    }

    // Update is called once per frame
    void Update()
    {
        animatorController.SetBool("EasterEgg", false);

        if (Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.P) && !audioSource.isPlaying)
        {
            animatorController.SetBool("EasterEgg", true);
            light.color = lightColorParty;
            light.intensity = 10;
            party = true;
            audioSource.Play();
        }

        if (party == true)
        {
            Debug.Log(elapsedTime);
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime >= 15.5f && party == true)
        {
            light.color = lightColor;
            light.intensity = 1.8f;
            elapsedTime = 0;
            party = false;
        }
    }
}
