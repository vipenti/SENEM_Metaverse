using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to handle the appearance of the smart students
public class StudentApperance : MonoBehaviour
{    
    private List<SkinnedMeshRenderer> haircuts,
                                    uniforms,
                                    eyes,
                                    brows,
                                    beards,
                                    glasses;

    private SkinnedMeshRenderer selectedHaircut,
                                selectedUniform,
                                selectedEyes,
                                selectedBrows,
                                selectedBeard,
                                selectedGlasses;
                            
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

        selectedHaircut = SelectRandomMesh(haircuts);
        selectedUniform = SelectRandomMesh(uniforms);
        selectedEyes = SelectRandomMesh(eyes);
        selectedBrows = SelectRandomMesh(brows);
        selectedBeard = SelectRandomMesh(beards);
        selectedGlasses = SelectRandomMesh(glasses);

        SetRandomColors();        
    }

    private SkinnedMeshRenderer SelectMesh(List<SkinnedMeshRenderer> list, int selected)
    {

        SkinnedMeshRenderer selectedMesh = null;

        for (int i = 0; i < list.Count; i++)
        {
            if (i == selected)
            {
                list[i].enabled = true;
                selectedMesh = list[i];
            }
            else
            {
                list[i].enabled = false;
            }
        }

        return selectedMesh;
    }
    
    private SkinnedMeshRenderer SelectRandomMesh(List<SkinnedMeshRenderer> list)
    {
        int random = Random.Range(0, list.Count);
        return SelectMesh(list, random);
    }

    private void SetRandomColors(){
        foreach (Material m in selectedUniform.materials)
        {
            if (m.name.Equals("Trousers (Instance)"))
                m.color = colorData.GetUniformColors()[UnityEngine.Random.Range(0, colorData.GetUniformColors().Count)];

            else if (m.name.Equals("Eyecolor (Instance)"))
                m.color = colorData.GetEyeColors()[UnityEngine.Random.Range(0, colorData.GetEyeColors().Count)];

            else if (m.name.Equals("Skin (Instance)"))
                m.color = colorData.GetSkinTones()[UnityEngine.Random.Range(0, colorData.GetSkinTones().Count)];

            else if (m.name.Equals("Tie (Instance)"))
                m.color = colorData.GetTieColors()[UnityEngine.Random.Range(0, colorData.GetTieColors().Count)];

            else if (m.name.Equals("Lipstick (Instance)"))
                m.color = colorData.GetLipsColors()[UnityEngine.Random.Range(0, colorData.GetLipsColors().Count)];
        }

        List<Color32> hairColors = colorData.GetHairColors();

        selectedHaircut.material.color = hairColors[UnityEngine.Random.Range(0, hairColors.Count)];
        selectedBrows.material.color = hairColors[UnityEngine.Random.Range(0, hairColors.Count)];
        selectedBeard.material.color = hairColors[UnityEngine.Random.Range(0, hairColors.Count)];

        foreach (Material m in selectedGlasses.materials)
        {
            if (m.name.Equals("Glasses (Instance)"))
                m.color = colorData.GetGlassesColors()[UnityEngine.Random.Range(0, colorData.GetGlassesColors().Count)];
        }
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

}
