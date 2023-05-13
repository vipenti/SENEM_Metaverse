using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

public class TabletSpawner : MonoBehaviourPunCallbacks
{
    public GameObject tabletPrefab;

    private Transform spawnPoint;
    public GameObject tablet;

    public void Start()
    {
        spawnPoint = GameObject.Find("SpawnPosition").transform;
        tablet = PhotonNetwork.Instantiate(tabletPrefab.name, spawnPoint.position, spawnPoint.rotation);
        tablet.transform.Rotate(0, 177.9f, 0);

        tablet.SetActive(false);

    }

    public void Update()
    {
    }

    public void SetTabletActive(bool active, Vector3 position)
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(RPCSetTabletActive), RpcTarget.All, active, position.x, position.y, position.z);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetTabletActive(tablet.activeSelf, tablet.transform.position);
    }

    [PunRPC]
    private void RPCSetTabletActive(bool active, float x, float y, float z)
    {
        Vector3 position = new Vector3(x, y, z);
        tablet.transform.position = position;
        tablet.SetActive(active);
    }
}
