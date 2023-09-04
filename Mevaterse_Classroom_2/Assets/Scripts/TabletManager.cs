using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Photon.Pun;
using System;

public class TabletManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField writeSpace;
    public bool isBeingEdited;
    private GameObject commandInfo;
    private TextChat textChat;

    private void Start()
    {
        writeSpace.readOnly = true;

        isBeingEdited = false;
        commandInfo = GameObject.Find("CommandInfo");

        writeSpace.text = $"{PhotonNetwork.NickName}'s notes, {DateTime.UtcNow.Date.ToString("MM/dd/yyyy")}";
        writeSpace.caretPosition = writeSpace.text.Length;

        textChat = GameObject.Find("TextChat").GetComponent<TextChat>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Space) && !isBeingEdited && !textChat.isSelected)
        {
            writeSpace.readOnly = false;
            EventSystem.current.SetSelectedGameObject(writeSpace.gameObject);
            Cursor.lockState = CursorLockMode.None;
            writeSpace.caretPosition = writeSpace.text.Length;
            commandInfo.SetActive(false);
            isBeingEdited = true;
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && isBeingEdited)
        {
            writeSpace.readOnly = true;
            EventSystem.current.SetSelectedGameObject(null);
            Cursor.lockState = CursorLockMode.Locked;
            LogManager.Instance.SaveNotes(writeSpace.text);
            commandInfo.SetActive(true);
            isBeingEdited = false;
        }

    }
}
