using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogClose : MonoBehaviour
{
    private int state = 0;
    public TMP_Text text;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X) && state == 0)
        {
            text.text = "<i>WARNING!</i>\nThis version in still a prototype and it's currently under development. There are some known issues that we " +
                "are currently trying to fix. Please, follow these advices for a better experience: connect your microphone device before joining the room. " +
                "If you want to change it or reset it, please leave the room and join again; for every wrong behaviour of your character or the platform, please " +
                "leave the room and join again. This will probably fix every known issue. Thank you for you patience.";
            state++;
        }

        else if((Input.GetKeyUp(KeyCode.X) && state == 1)) {

            this.gameObject.SetActive(false);
        }
    }
}
