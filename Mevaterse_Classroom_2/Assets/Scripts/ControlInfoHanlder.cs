using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlInfoHanlder : MonoBehaviour
{
    private Image background;
    private TMP_Text text;
    private void Start()
    {
        background = gameObject.GetComponent<Image>();
        text = gameObject.GetComponentInChildren<TMP_Text>();

        background.enabled = false;
        text.enabled = false;
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Z)) {
            background.enabled = !background.enabled;
            text.enabled = !text.enabled;
        }
    }
}
