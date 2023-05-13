using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class TextChat : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputField;
    public bool isSelected = false;

    public void LateUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Return) && !isSelected)
        {
            isSelected = true;
            // Set the selected GameObject to the input field
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);

            // Optionally, move the caret to the end of the input field's text
            inputField.caretPosition = inputField.text.Length;
        }

        else if(Input.GetKeyUp(KeyCode.Escape) && isSelected)
        {
            isSelected = false;
            // Reset the selected GameObject 
            EventSystem.current.SetSelectedGameObject(null);
        }

        else if (Input.GetKeyUp(KeyCode.Return) && isSelected && inputField.text != "")
        {
            photonView.RPC("SendMessageRpc", RpcTarget.AllBuffered, PhotonNetwork.NickName, inputField.text);
            inputField.text = "";
            isSelected = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    [PunRPC]
    public void SendMessageRpc(string sender, string msg)
    {
        string message = $"<color=\"yellow\">{sender}</color>: {msg}";
        Logger.Instance.LogInfo(message);
        LogManager.Instance.LogInfo($"{sender} wrote in the chat: \"{msg}\"");
    }
}