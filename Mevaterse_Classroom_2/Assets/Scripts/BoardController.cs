using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BoardController : MonoBehaviourPunCallbacks, IPunObservable
{
    private List<Material> slides = new List<Material>();
    private string path = "Images/Materials";
    private int current = 0;
    void Start()
    {
        Object[] materialObjects = Resources.LoadAll(path, typeof(Object));

        foreach (Object obj in materialObjects)
        {
            Material mat = obj as Material;
            if (mat != null)
            {
                slides.Add(mat);
                current += 1;
            }
        }
    }
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.UserId != Presenter.Instance.presenterID) return;

        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            photonView.RPC("ChangeSlideRpc", RpcTarget.All, +1);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            photonView.RPC("ChangeSlideRpc", RpcTarget.All, -1);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("ChangeSlideRpc", RpcTarget.All, +1);
        photonView.RPC("ChangeSlideRpc", RpcTarget.All, -1);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // This is the owner of the object; send the variable value to other clients
            stream.SendNext(current);
        }
        else
        {
            // This is a non-owner client; receive the variable value from the owner
            current = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void ChangeSlideRpc(int value)
    {
        current += value;

        if (current >= slides.Count)
        {
            current = 0;
        }

        if (current < 0)
        {
            current = slides.Count - 1;
        }

        GetComponent<Renderer>().material = slides[current];
    }
}
