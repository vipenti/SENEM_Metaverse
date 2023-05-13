using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogClose : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
            this.gameObject.SetActive(false);
    }
}
