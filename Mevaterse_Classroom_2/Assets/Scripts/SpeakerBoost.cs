using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class SpeakerBoost : MonoBehaviourPunCallbacks
{
    public bool near = false;
    public AudioSource speaker;

    public float defaultVolume;
    public float defaultSpatialBlend;

    public TMP_Text interactionInfo;

    private void Start()
    {
        interactionInfo = GameObject.Find("InteractionInfo2").GetComponent<TMP_Text>();
        interactionInfo.text = "";
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (near)
        {
            photonView.RPC("AudioBoost", RpcTarget.All, 1f, 0.5f);
        }

        else
        {
            photonView.RPC("AudioBoost", RpcTarget.All, defaultVolume, defaultSpatialBlend);
        }

        if (near && Input.GetKeyUp(KeyCode.P) && PhotonNetwork.LocalPlayer.UserId != Presenter.Instance.presenterID)
        {
            photonView.RPC("SwitchPresenter", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId);
            photonView.RPC("AnnouncePresenter", RpcTarget.All, GetComponent<PhotonView>().Controller.NickName);
        }

        InteractionUpdate();
    }

    private void InteractionUpdate()
    {
        if (near && PhotonNetwork.LocalPlayer.UserId != Presenter.Instance.presenterID)
            interactionInfo.text = "Press P to take control of the slides";

        else if (near && PhotonNetwork.LocalPlayer.UserId == Presenter.Instance.presenterID)
            interactionInfo.text = "You're the presenter";
        else
            interactionInfo.text = "";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!photonView.IsMine) return;
        photonView.RPC("SwitchPresenter", RpcTarget.All, Presenter.Instance.presenterID);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("AudioBoost"))
        {
            near = true;
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("AudioBoost"))
        {
            near = false;
        }
    }

    [PunRPC]
    public void AudioBoost(float volume, float spatialBlend)
    {
        speaker.volume = volume;
        speaker.spatialBlend = spatialBlend;
    }

    [PunRPC]
    public void SwitchPresenter(string presenterID)
    {
        Presenter.Instance.presenterID = presenterID;
    }

    [PunRPC]
    public void AnnouncePresenter(string name)
    {
        LogManager.Instance.LogInfo($"Presenter is now {name}");
        Logger.Instance.LogInfo($"Presenter is now <color=yellow>{name}</color>");
    }
}
