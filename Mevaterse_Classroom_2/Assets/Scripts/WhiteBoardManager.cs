using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Photon.Pun;
using System;
using Photon.Realtime;

public class WhiteBoardManager : MonoBehaviourPunCallbacks
{
    public bool isBeingEdited;
    public bool playerNear;

    public TMP_InputField writeSpace;
    public TMP_Text interactionInfo;
    public GameObject commandInfo;
    public TextChat textChat;

    private string text;

    private void Start()
    {
        writeSpace.readOnly = true;
        isBeingEdited = false;
        interactionInfo.text = "";
        writeSpace.text = $"{DateTime.UtcNow.Date.ToString("MM/dd/yyyy")}";
        writeSpace.caretPosition = writeSpace.text.Length;
    }

    void LateUpdate()
    {
        if (!photonView.IsMine) return; 

        if (Input.GetKeyUp(KeyCode.Space) && playerNear && Presenter.Instance.writerID == "none" && !isBeingEdited && !textChat.isSelected)
        {
            BeginEditBorad();
            photonView.RPC("LockBoard", RpcTarget.All, true, PhotonNetwork.LocalPlayer.UserId);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && isBeingEdited && playerNear 
            && PhotonNetwork.LocalPlayer.UserId == Presenter.Instance.writerID && !textChat.isSelected)
        {
            EndEditBoard();
            photonView.RPC("LockBoard", RpcTarget.All, false, "none");
            text = writeSpace.text;
            photonView.RPC("SetText", RpcTarget.All, text);
        }
        InteractionUpdate();
    }

    private void BeginEditBorad()
    {
        writeSpace.readOnly = false;
        EventSystem.current.SetSelectedGameObject(writeSpace.gameObject);
        Cursor.lockState = CursorLockMode.None;
        writeSpace.caretPosition = writeSpace.text.Length;
        commandInfo.GetComponent<ControlInfoHanlder>().enabled = false;
    }

    private void EndEditBoard()
    {
        writeSpace.readOnly = true;
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        LogManager.Instance.SaveNotes(writeSpace.text);
        commandInfo.GetComponent<ControlInfoHanlder>().enabled = true;
    }

    private void InteractionUpdate()
    {
        if (playerNear && !isBeingEdited && Presenter.Instance.writerID == "none")
            interactionInfo.text = "Press SPACE to start writing on the whiteboard";

        else if (playerNear && isBeingEdited && PhotonNetwork.LocalPlayer.UserId != Presenter.Instance.writerID)
            interactionInfo.text = "Whiteboard is busy";

        else if(playerNear && isBeingEdited && PhotonNetwork.LocalPlayer.UserId == Presenter.Instance.writerID)
            interactionInfo.text = "Press ESC to stop writing";
        else
            interactionInfo.text = "";
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!photonView.IsMine) return;
        photonView.RPC("LockBoard", RpcTarget.All, Presenter.Instance.writerID == "none", Presenter.Instance.writerID);
        photonView.RPC("SetText", RpcTarget.All, writeSpace.text);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNear = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerNear = false;
    }

    [PunRPC]
    public void LockBoard(bool value, string id)
    {
        isBeingEdited = value;
        Presenter.Instance.writerID = id;
    }

    [PunRPC]
    public void SetText(string msg)
    {
        writeSpace.text = msg;
    }
}
