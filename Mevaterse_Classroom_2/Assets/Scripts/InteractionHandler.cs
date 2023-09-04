using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text interactionInfo;

    void LateUpdate()
    {
        if (interactionInfo.text == "")
            gameObject.GetComponent<Image>().enabled = false;
        else
            gameObject.GetComponent<Image>().enabled = true;
    }
}
