using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Class to handle the appearance of the smart students
public class StudentApperance : MonoBehaviourPun
{    
    private List<SkinnedMeshRenderer> haircuts,
                                    uniforms,
                                    eyes,
                                    brows,
                                    beards,
                                    glasses;

    private StudentData studentData;
    
    private ColorData colorData;

    void Start()
    {
        colorData = GameObject.Find("ColorData").GetComponent<ColorData>();;

        haircuts = new List<SkinnedMeshRenderer>();
        uniforms = new List<SkinnedMeshRenderer>();
        eyes = new List<SkinnedMeshRenderer>();
        brows = new List<SkinnedMeshRenderer>();
        beards = new List<SkinnedMeshRenderer>();
        glasses = new List<SkinnedMeshRenderer>();

        LoadMeshes(GetComponentsInChildren<Transform>());

        studentData = new StudentData();

        if(photonView.IsMine)
        {   
            studentData.selectedHaircut = SelectRandomMesh(haircuts);
            studentData.selectedUniform = SelectRandomMesh(uniforms);
            studentData.selectedEyes = SelectRandomMesh(eyes);
            studentData.selectedBrows = SelectRandomMesh(brows);
            studentData.selectedBeard = SelectRandomMesh(beards);
            studentData.selectedGlasses = SelectRandomMesh(glasses);

            SetRandomColors();

            string studentString = JsonUtility.ToJson(studentData);
            photonView.RPC("SetStudentData", RpcTarget.OthersBuffered, (string)studentString);
        }      
    }

    [PunRPC]
    public void SetStudentData(string studentString)
    {
        studentData = JsonUtility.FromJson<StudentData>(studentString);

        LoadMeshes(GetComponentsInChildren<Transform>());

        SelectMesh(haircuts, studentData.selectedHaircut);
        SelectMesh(uniforms, studentData.selectedUniform);
        SelectMesh(eyes, studentData.selectedEyes);
        SelectMesh(brows, studentData.selectedBrows);
        SelectMesh(beards, studentData.selectedBeard);
        SelectMesh(glasses, studentData.selectedGlasses);

        SetColors();
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
    
    private int SelectRandomMesh(List<SkinnedMeshRenderer> list)
    {
        int random = Random.Range(0, list.Count);
        SelectMesh(list, random);

        return random;
    }

    private void SetColors()
    {
        foreach (Material m in uniforms[studentData.selectedUniform].materials)
        {
            if (m.name.Equals("Trousers (Instance)"))
            {
                m.color = studentData.uniformColor;
            }

            else if (m.name.Equals("Eyecolor (Instance)"))
            {
                m.color = studentData.eyeColor;
            }

            else if (m.name.Equals("Skin (Instance)"))
            {
                m.color = studentData.skinColor;
            }

            else if (m.name.Equals("Tie (Instance)"))
            {
                m.color = studentData.tieColor;
            }

            else if (m.name.Equals("Lipstick (Instance)"))
            {
                m.color = studentData.lipsColor;
            }
        }

        haircuts[studentData.selectedHaircut].material.color = studentData.hairColor;
        brows[studentData.selectedBrows].material.color = studentData.hairColor;
        beards[studentData.selectedBeard].material.color = studentData.hairColor;

        foreach (Material m in glasses[studentData.selectedGlasses].materials)
        {
            if (m.name.Equals("Glasses (Instance)"))
            {
                m.color = studentData.glassesColor;
            }
        }
    }

    private void SetRandomColors(){
        foreach (Material m in uniforms[studentData.selectedUniform].materials)
        {
            if (m.name.Equals("Trousers (Instance)"))
            {
                m.color = colorData.GetUniformColors()[UnityEngine.Random.Range(0, colorData.GetUniformColors().Count)];
                studentData.uniformColor = m.color;
            }

            else if (m.name.Equals("Eyecolor (Instance)"))
            {
                m.color = colorData.GetEyeColors()[UnityEngine.Random.Range(0, colorData.GetEyeColors().Count)];
                studentData.eyeColor = m.color;
            }

            else if (m.name.Equals("Skin (Instance)"))
            {
                m.color = colorData.GetSkinTones()[UnityEngine.Random.Range(0, colorData.GetSkinTones().Count)];
                studentData.skinColor = m.color;
            }

            else if (m.name.Equals("Tie (Instance)"))
            {
                m.color = colorData.GetTieColors()[UnityEngine.Random.Range(0, colorData.GetTieColors().Count)];
                studentData.tieColor = m.color;
            }

            else if (m.name.Equals("Lipstick (Instance)"))
            {
                m.color = colorData.GetLipsColors()[UnityEngine.Random.Range(0, colorData.GetLipsColors().Count)];
                studentData.lipsColor = m.color;
            }
        }

        studentData.hairColor = colorData.GetHairColors()[UnityEngine.Random.Range(0, colorData.GetHairColors().Count)];

        haircuts[studentData.selectedHaircut].material.color = studentData.hairColor;
        brows[studentData.selectedBrows].material.color = studentData.hairColor;
        beards[studentData.selectedBeard].material.color = studentData.hairColor;

        foreach (Material m in glasses[studentData.selectedGlasses].materials)
        {
            if (m.name.Equals("Glasses (Instance)"))
            {
                m.color = colorData.GetGlassesColors()[UnityEngine.Random.Range(0, colorData.GetGlassesColors().Count)];
                studentData.glassesColor = m.color;
            }
        }
    }

    private void LoadMeshes(Transform[] children)
    {
        if(haircuts == null)
        {
            haircuts = new List<SkinnedMeshRenderer>();
        }

        if(uniforms == null)
        {
            uniforms = new List<SkinnedMeshRenderer>();
        }

        if(eyes == null)
        {
            eyes = new List<SkinnedMeshRenderer>();
        }

        if(brows == null)
        {
            brows = new List<SkinnedMeshRenderer>();
        }

        if(beards == null)
        {
            beards = new List<SkinnedMeshRenderer>();
        }

        if(glasses == null)
        {
            glasses = new List<SkinnedMeshRenderer>();
        }

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

}

class StudentData{
    public int selectedHaircut,
            selectedUniform,
            selectedEyes,
            selectedBrows,
            selectedBeard,
            selectedGlasses;
    
    public Color32 hairColor,
                uniformColor,
                eyeColor,
                skinColor,
                tieColor,
                lipsColor,
                glassesColor;
}
