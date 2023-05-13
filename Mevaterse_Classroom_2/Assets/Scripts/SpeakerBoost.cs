using UnityEngine;
using Photon.Pun;

public class SpeakerBoost : MonoBehaviourPunCallbacks
{
    public bool near = false;
    public AudioSource speaker;

    public float defaultVolume;
    public float defaultSpatialBlend;

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (near)
            photonView.RPC("AudioBoost", RpcTarget.All, 1f, 0.5f);
        else 
            photonView.RPC("AudioBoost", RpcTarget.All, defaultVolume, defaultSpatialBlend);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("AudioBoost"))
        {
            near = true;
            Debug.Log("Near leggio");
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("AudioBoost"))
        {
            near = false;
            Debug.Log("Near leggio");
        }
    }

    [PunRPC]
    public void AudioBoost(float volume, float spatialBlend)
    {
        speaker.volume = volume;
        speaker.spatialBlend = spatialBlend;
    }
}
