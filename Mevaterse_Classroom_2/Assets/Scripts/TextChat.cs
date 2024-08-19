using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class TextChat : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputField;
    public bool isSelected = false;
    private GameObject commandInfo;
    private AudioSource audioSource;

    private void Start()
    {
        commandInfo = GameObject.Find("CommandInfo");
        audioSource = GetComponent<AudioSource>();
    }

    public void LateUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Return) && !isSelected)
        {
            isSelected = true;
            // Set the selected GameObject to the input field
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
            inputField.caretPosition = inputField.text.Length;
            commandInfo.SetActive(false);
        }

        else if(Input.GetKeyUp(KeyCode.Escape) && isSelected)
        {
            isSelected = false;
            // Reset the selected GameObject 
            EventSystem.current.SetSelectedGameObject(null);
            commandInfo.SetActive(true);
        }

        else if (Input.GetKeyUp(KeyCode.Return) && isSelected && inputField.text != "")
        {
            photonView.RPC("SendMessageRpc", RpcTarget.AllBuffered, PhotonNetwork.NickName, inputField.text, true);
            inputField.text = "";
            isSelected = false;
            EventSystem.current.SetSelectedGameObject(null);
            commandInfo.SetActive(true);
        }
    }

    [PunRPC]
    public void SendMessageRpc(string sender, string msg, bool notify = false)
    {
        string message = $"<color=\"yellow\">{sender}</color>: {msg}";
        Logger.Instance.LogInfo(message);
        LogManager.Instance.LogInfo($"{sender} wrote in the chat: \"{msg}\"");
        
        if (notify)
        {
            audioSource.Play();
        }
    }
}