using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChairController : MonoBehaviourPunCallbacks, IPunObservable
{
    private bool isBusy;
    public string playerName = "";

    private PhotonView view;
    private void Start()
    {
        isBusy = false;
        view = this.GetComponent<PhotonView>();
    }
    public override void OnPlayerLeftRoom(Player player)
    {
        if(player.NickName == playerName)
        {
            view.RPC("VerifySeatState", RpcTarget.All);
        }
    }
    public bool IsBusy()
    {        
        return isBusy;
    }

    public void SetBusy(bool value)
    {
        isBusy = value;
    }

    // These RPCs synchronize all the chairs states
    [PunRPC]
    public void VerifySeatState()
    {
        SetBusy(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // This is the owner of the object; send the variable value to other clients
            stream.SendNext(isBusy);
            stream.SendNext(playerName);
        }
        else
        {
            // This is a non-owner client; receive the variable value from the owner
            isBusy = (bool)stream.ReceiveNext();
            playerName = (string)stream.ReceiveNext();
        }
    }
}
