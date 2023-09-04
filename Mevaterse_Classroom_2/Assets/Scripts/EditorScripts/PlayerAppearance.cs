using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviourPunCallbacks
{
    private List<SkinnedMeshRenderer> haircuts;
    private List<SkinnedMeshRenderer> uniforms;
    private List<SkinnedMeshRenderer> eyes;
    private List<SkinnedMeshRenderer> brows;
    private List<SkinnedMeshRenderer> beards;
    private List<SkinnedMeshRenderer> glasses;

    private void Awake()
    {
        if (!photonView.IsMine) return;

        photonView.RPC("SetMeshes", RpcTarget.All, AvatarSettings.Instance.selectedHaircut, AvatarSettings.Instance.selectedUniform,
            AvatarSettings.Instance.selectedGlasses, AvatarSettings.Instance.selectedBeard, AvatarSettings.Instance.selectedEyebrows,
            AvatarSettings.Instance.selectedLashes);
        photonView.RPC("SetColors", RpcTarget.All, AvatarSettings.Instance.colorSettings, AvatarSettings.Instance.selectedHaircut, 
            AvatarSettings.Instance.selectedUniform, AvatarSettings.Instance.selectedGlasses, AvatarSettings.Instance.selectedBeard, 
            AvatarSettings.Instance.selectedEyebrows, AvatarSettings.Instance.selectedLashes);        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!photonView.IsMine) return;
        photonView.RPC("SetMeshes", RpcTarget.All, AvatarSettings.Instance.selectedHaircut, AvatarSettings.Instance.selectedUniform,
            AvatarSettings.Instance.selectedGlasses, AvatarSettings.Instance.selectedBeard, AvatarSettings.Instance.selectedEyebrows,
            AvatarSettings.Instance.selectedLashes);
        photonView.RPC("SetColors", RpcTarget.All, AvatarSettings.Instance.colorSettings, AvatarSettings.Instance.selectedHaircut,
            AvatarSettings.Instance.selectedUniform, AvatarSettings.Instance.selectedGlasses, AvatarSettings.Instance.selectedBeard,
            AvatarSettings.Instance.selectedEyebrows, AvatarSettings.Instance.selectedLashes);
    }

    private void LoadMeshes(Transform[] children)
    {
        foreach (Transform child in children)
        {
            if (child.CompareTag("Haircut"))
                haircuts.Add(child.GetComponent<SkinnedMeshRenderer>());

            else if (child.CompareTag("Beard"))
                beards.Add(child.GetComponent<SkinnedMeshRenderer>());

            else if (child.CompareTag("Eyes"))
                eyes.Add(child.GetComponent<SkinnedMeshRenderer>());

            else if (child.CompareTag("Brows"))
                brows.Add(child.GetComponent<SkinnedMeshRenderer>());

            else if (child.CompareTag("Glasses"))
                glasses.Add(child.GetComponent<SkinnedMeshRenderer>());

            else if (child.CompareTag("Uniform"))
                uniforms.Add(child.GetComponent<SkinnedMeshRenderer>());
        }

    }
    [PunRPC]
    public void SetMeshes(int selectedHaircut, int selectedUniform, int selectedGlasses, int selectedBeard, int selectedBrows, int selectedEyes)
    {
        haircuts = new List<SkinnedMeshRenderer>();
        uniforms = new List<SkinnedMeshRenderer>();
        eyes = new List<SkinnedMeshRenderer>();
        brows = new List<SkinnedMeshRenderer>();
        beards = new List<SkinnedMeshRenderer>();
        glasses = new List<SkinnedMeshRenderer>();

        LoadMeshes(this.photonView.GetComponentsInChildren<Transform>());

        SelectMesh(uniforms, selectedUniform);
        SelectMesh(haircuts, selectedHaircut);
        SelectMesh(eyes, selectedEyes);
        SelectMesh(brows, selectedBrows);
        SelectMesh(glasses, selectedGlasses);
        SelectMesh(beards, selectedBeard);
    }

    private void SelectMesh(List<SkinnedMeshRenderer> list, int selected)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (i == selected)
            {
                list[i].enabled = true;

            }
            else
            {
                list[i].enabled = false;
            }
        }
    }

    [PunRPC]
    public void SetColors(Hashtable colors, int selectedHaircut, int selectedUniform, int selectedGlasses, int selectedBeard, int selectedBrows, int selectedEyes)
    {
        foreach (Material m in uniforms[selectedUniform].materials)
        {
            if (m.name.Equals("Trousers (Instance)"))
                m.color = (Color32)colors["uniform"];

            else if (m.name.Equals("Eyecolor (Instance)"))
                m.color = (Color32)colors["eyes"];

            else if (m.name.Equals("Skin (Instance)"))
                m.color = (Color32)colors["skin"];

            else if (m.name.Equals("Tie (Instance)"))
                m.color = (Color32)colors["tie"];

            else if (m.name.Equals("Lipstick (Instance)"))
                m.color = (Color32)colors["lips"];
        }

        haircuts[selectedHaircut].material.color = (Color32)colors["hair"];
        brows[selectedBrows].material.color = (Color32)colors["hair"];
        beards[selectedBeard].material.color = (Color32)colors["hair"];

        foreach (Material m in glasses[selectedGlasses].materials)
        {
            if (m.name.Equals("Glasses (Instance)"))
                m.color = (Color32)colors["glasses"];
        }

    }
}
