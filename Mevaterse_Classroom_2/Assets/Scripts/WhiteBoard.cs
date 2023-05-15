using Photon.Pun;
using System;
using TMPro;

public class WhiteBoard : MonoBehaviourPunCallbacks
{
    public TMP_InputField boardText;
    public bool isBeingEdited = false;

    private void Start()
    {
        boardText.text = $"{DateTime.UtcNow.Date.ToString("MM/dd/yyyy")}";
        boardText.readOnly = true;
    }
    public void Edited(bool value)
    {
        photonView.RPC("SetEdit", RpcTarget.All, value);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // This is the owner of the object; send the variable value to other clients
            stream.SendNext(isBeingEdited);
            stream.SendNext(boardText.text);
        }
        else
        {
            // This is a non-owner client; receive the variable value from the owner
            isBeingEdited = (bool)stream.ReceiveNext();
            boardText.text = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void SetEdit(bool value)
    {
        isBeingEdited = value;
    }
}
