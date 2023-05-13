using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    public Button loadScene;
    private int buttonSize = 30;

    [Header("Containers")]
    public GameObject uniformContainer;
    public GameObject lashesContainer;
    public GameObject hairContainer;
    public GameObject beardContainer;
    public GameObject eyebrowsContainer;
    public GameObject glassesContainer;

    [Header("ColorPanels")]
    public Transform uniformPanel;
    public Transform tiePanel;
    public Transform skinPanel;
    public Transform lipsPanel;
    public Transform eyesPanel;
    public Transform hairPanel;
    public Transform glassesPanel;

    [Header("Mesh lists")]
    private List<SkinnedMeshRenderer> uniformsList;
    private List<SkinnedMeshRenderer> haircutsList;
    private List<SkinnedMeshRenderer> eyebrowsList;
    private List<SkinnedMeshRenderer> lashesList;
    private List<SkinnedMeshRenderer> beardsList;
    private List<SkinnedMeshRenderer> glassesList;

    [Header("Button lists")]
    private List<Color32> uniform_colors;
    private List<Color32> tie_colors;
    private List<Color32> skin_tones;
    private List<Color32> lips_colors;
    private List<Color32> eye_colors;
    private List<Color32> hair_colors;
    private List<Color32> glasses_colors;

    [Header("Button prefab")]
    public GameObject prefabButton;

    [Header("Button previous & next")]
    public Button previousUniform;
    public Button nextUniform;

    public Button previousEyes;
    public Button nextEyes;

    public Button previousHair;
    public Button nextHair;

    public Button previousBrows;
    public Button nextBrows;

    public Button previousBeard;
    public Button nextBeard;

    public Button previousGlasses;
    public Button nextGlasses;

    [Header("Selected indexes")]
    private int selectedHaircut;
    private int selectedUniform;
    private int selectedEyebrows;
    private int selectedLashes;
    private int selectedGlasses;
    private int selectedBeard;

    [Header("Selected text indicator")]
    public TMP_Text selectedHaircutTxt;
    public TMP_Text selectedUniformTxt;
    public TMP_Text selectedEyebrowsTxt;
    public TMP_Text selectedLashesTxt;
    public TMP_Text selectedGlassesTxt;
    public TMP_Text selectedBeardTxt;

    void Start()
    {
        // ---------------------------------------------- INITIALIZATION ---------------------------------------------------------- //
        uniformsList = new List<SkinnedMeshRenderer>();
        haircutsList = new List<SkinnedMeshRenderer>();
        eyebrowsList = new List<SkinnedMeshRenderer>();
        beardsList = new List<SkinnedMeshRenderer>();
        lashesList = new List<SkinnedMeshRenderer>();
        glassesList = new List<SkinnedMeshRenderer>();

        FillLists(uniformContainer, uniformsList);
        FillLists(hairContainer, haircutsList);
        FillLists(lashesContainer, lashesList);
        FillLists(glassesContainer, glassesList);
        FillLists(beardContainer, beardsList);
        FillLists(eyebrowsContainer, eyebrowsList);

        selectedHaircut = AvatarSettings.Instance.selectedHaircut;
        selectedGlasses = AvatarSettings.Instance.selectedGlasses;
        selectedUniform = AvatarSettings.Instance.selectedUniform;
        selectedEyebrows = AvatarSettings.Instance.selectedEyebrows;
        selectedLashes = AvatarSettings.Instance.selectedLashes;
        selectedBeard = AvatarSettings.Instance.selectedBeard;

        selectedHaircutTxt.text = selectedHaircut.ToString();
        selectedGlassesTxt.text = selectedGlasses.ToString();
        selectedUniformTxt.text = selectedUniform.ToString();
        selectedEyebrowsTxt.text = selectedEyebrows.ToString();
        selectedLashesTxt.text = selectedLashes.ToString();
        selectedBeardTxt.text = selectedBeard.ToString();

        // Initialize mesh from Avatar Settings
        ChangeMesh(uniformsList, selectedUniform, 0, selectedUniformTxt);
        ChangeMesh(haircutsList, selectedHaircut, 0, selectedHaircutTxt);
        ChangeMesh(glassesList, selectedGlasses, 0, selectedGlassesTxt);
        ChangeMesh(beardsList, selectedBeard, 0, selectedBeardTxt);
        ChangeMesh(lashesList, selectedLashes, 0, selectedLashesTxt);
        ChangeMesh(eyebrowsList, selectedEyebrows, 0, selectedEyebrowsTxt);

        // Initialize material color from Avatar Settings
        UpdateMaterial(haircutsList, "Hair", "hair");
        UpdateMaterial(eyebrowsList, "Hair", "hair");
        UpdateMaterial(beardsList, "Hair", "hair");
        UpdateMaterial(uniformsList, "Trousers", "uniform");
        UpdateMaterial(uniformsList, "Skin", "skin");
        UpdateMaterial(uniformsList, "Eyecolor", "eyes");
        UpdateMaterial(uniformsList, "Tie", "tie");
        UpdateMaterial(uniformsList, "Lipstick", "lips");
        UpdateMaterial(glassesList, "Glasses", "glasses");

        // Pre-defined color for each section
        InitializeUniformColors();
        InitializeTieColors();
        InitializeSkinTones();
        InitializeLipsColors();
        InitializeEyeColors();
        InitializeHairColors();
        InitializeGlassesColors();

        // Generator of buttons for each pre-defined color section
        ButtonsGenerator(26, 13, 1.2f, buttonSize, 218, 15, skin_tones, skinPanel, "Skin", uniformsList, "skin");
        ButtonsGenerator(14, 7, 1.17f, buttonSize / 1.08f, 98, 15, uniform_colors, uniformPanel, "Trousers", uniformsList, "uniform");
        ButtonsGenerator(14, 7, 1.17f, buttonSize / 1.08f, 98, 15, tie_colors, tiePanel, "Tie", uniformsList, "tie");
        ButtonsGenerator(26, 13, 1.2f, buttonSize, 218, 15, lips_colors, lipsPanel, "Lipstick", uniformsList, "lips");
        ButtonsGenerator(26, 13, 1.2f, buttonSize / 1.02f, 211, 15, eye_colors, eyesPanel, "Eyecolor", uniformsList, "eyes");
        ButtonsGenerator(30, 10, 1.2f, buttonSize / 1.2f, 135, 25, hair_colors, hairPanel, "Hair", haircutsList, "hair");
        ButtonsGenerator(13, 13, 1.2f, buttonSize, 216, 2, glasses_colors, glassesPanel, "Glasses", glassesList, "glasses");

        // Previous & Next button setup
        InitializePreviousNextButtons();

        loadScene.onClick.AddListener(() => {

            AvatarSettings.Instance.selectedHaircut = selectedHaircut;
            AvatarSettings.Instance.selectedGlasses = selectedGlasses;
            AvatarSettings.Instance.selectedUniform = selectedUniform;
            AvatarSettings.Instance.selectedEyebrows = selectedEyebrows;
            AvatarSettings.Instance.selectedLashes = selectedLashes;
            AvatarSettings.Instance.selectedBeard = selectedBeard;
            SceneManager.LoadScene("ClassroomScene");
        });

    }

    private void FillLists(GameObject container, List<SkinnedMeshRenderer> list)
    {
        Transform[] children = container.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
        {
            list.Add(children[i].GetComponent<SkinnedMeshRenderer>());
        }
    }

    private void UpdateMaterial(List<SkinnedMeshRenderer> list, string materialName, string key)
    {
        materialName += " (Instance)";

        foreach (SkinnedMeshRenderer skinned in list)
        {
            foreach (Material mat in skinned.materials)
            {
                if (mat.name.Equals(materialName))
                {
                    mat.color = AvatarSettings.Instance.GetColorByKey(key);
                }
            }
        }
    }

    private void InitializePreviousNextButtons()
    {
        previousUniform.onClick.AddListener(() => {
            selectedUniform = ChangeMesh(uniformsList, selectedUniform, -1, selectedUniformTxt);
        });
        nextUniform.onClick.AddListener(() => {
            selectedUniform = ChangeMesh(uniformsList, selectedUniform, +1, selectedUniformTxt);
        });

        previousHair.onClick.AddListener(() => {
            selectedHaircut = ChangeMesh(haircutsList, selectedHaircut, -1, selectedHaircutTxt);
        });
        nextHair.onClick.AddListener(() => {
            selectedHaircut = ChangeMesh(haircutsList, selectedHaircut, +1, selectedHaircutTxt);
        });

        previousGlasses.onClick.AddListener(() => {
            selectedGlasses = ChangeMesh(glassesList, selectedGlasses, -1, selectedGlassesTxt);
        });
        nextGlasses.onClick.AddListener(() => {
            selectedGlasses = ChangeMesh(glassesList, selectedGlasses, +1, selectedGlassesTxt);
        });

        previousEyes.onClick.AddListener(() => {
            selectedLashes = ChangeMesh(lashesList, selectedLashes, -1, selectedLashesTxt);
        });
        nextEyes.onClick.AddListener(() => {
            selectedLashes = ChangeMesh(lashesList, selectedLashes, +1, selectedLashesTxt);
        });

        previousBrows.onClick.AddListener(() => {
            selectedEyebrows = ChangeMesh(eyebrowsList, selectedEyebrows, -1, selectedEyebrowsTxt);
        });
        nextBrows.onClick.AddListener(() => {
            selectedEyebrows = ChangeMesh(eyebrowsList, selectedEyebrows, +1, selectedEyebrowsTxt);
        });

        previousBeard.onClick.AddListener(() => {
            selectedBeard = ChangeMesh(beardsList, selectedBeard, -1, selectedBeardTxt);
        });
        nextBeard.onClick.AddListener(() => {
            selectedBeard = ChangeMesh(beardsList, selectedBeard, +1, selectedBeardTxt);
        });
    }

    private void InitializeUniformColors()
    {
        uniform_colors = new List<Color32>();

        uniform_colors.Add(new Color32(27, 27, 27, 255));    // Black
        uniform_colors.Add(new Color32(99, 99, 99, 255));    // Gray
        uniform_colors.Add(new Color32(200, 200, 200, 255)); // White
        uniform_colors.Add(new Color32(12, 16, 57, 255)); // Very dark blue
        uniform_colors.Add(new Color32(25, 52, 115, 255));   // Dark blue
        uniform_colors.Add(new Color32(91, 134, 229, 255));  // Light blue
        uniform_colors.Add(new Color32(43, 96, 47, 255));   // Dark Green
        uniform_colors.Add(new Color32(68, 159, 70, 255));   // Green
        uniform_colors.Add(new Color32(0, 137, 123, 255)); // Aquamarine
        uniform_colors.Add(new Color32(230, 165, 45, 255));  // Orange
        uniform_colors.Add(new Color32(244, 147, 180, 255));  // Pink
        uniform_colors.Add(new Color32(177, 60, 60, 255));   // Red
        uniform_colors.Add(new Color32(123, 39, 39, 255));   // Dark red
        uniform_colors.Add(new Color32(139, 69, 19, 255));   // Brown 

    }
    private void InitializeTieColors()
    {
        tie_colors = new List<Color32>();

        tie_colors.Add(new Color32(110, 166, 66, 255));   // Bright green
        tie_colors.Add(new Color32(32, 115, 41, 255));    // Forest green
        tie_colors.Add(new Color32(34, 42, 119, 255));   // Dark Blue
        tie_colors.Add(new Color32(20, 94, 179, 255));   // Blue
        tie_colors.Add(new Color32(85, 160, 186, 255));   // Sky blue
        tie_colors.Add(new Color32(120, 62, 136, 255));   // Purple
        tie_colors.Add(new Color32(227, 111, 163, 255));  // Pink
        tie_colors.Add(new Color32(235, 100, 0, 255));    // Orange-red
        tie_colors.Add(new Color32(235, 78, 0, 255));     // Bright orange
        tie_colors.Add(new Color32(190, 0, 0, 255));      // Red
        tie_colors.Add(new Color32(131, 21, 21, 255));    // Dark red
        tie_colors.Add(new Color32(27, 27, 27, 255)); // Black
        tie_colors.Add(new Color32(99, 99, 99, 255));    // Gray
        tie_colors.Add(new Color32(255, 255, 255, 255)); // White

    }

    private void InitializeSkinTones()
    {
        skin_tones = new List<Color32>();

        skin_tones.Add(new Color32(255, 240, 215, 255)); // Ivory
        skin_tones.Add(new Color32(245, 221, 203, 255)); // Fair
        skin_tones.Add(new Color32(229, 195, 166, 255)); // Porcelain
        skin_tones.Add(new Color32(209, 161, 129, 255)); // Light Beige   
        skin_tones.Add(new Color32(215, 169, 140, 255)); // Almond
        skin_tones.Add(new Color32(194, 145, 98, 255)); // Nude
        skin_tones.Add(new Color32(194, 143, 107, 255)); // Chestnut
        skin_tones.Add(new Color32(169, 118, 76, 255)); // Cocoa
        skin_tones.Add(new Color32(181, 118, 67, 255)); // Honey
        skin_tones.Add(new Color32(143, 91, 42, 255)); // Espresso
        skin_tones.Add(new Color32(114, 67, 34, 255)); // Mocha
        skin_tones.Add(new Color32(99, 58, 30, 255)); // Sable
        skin_tones.Add(new Color32(255, 223, 196, 255)); // Peach
        skin_tones.Add(new Color32(249, 192, 169, 255)); // Rose
        skin_tones.Add(new Color32(238, 163, 144, 255)); // Sand        
        skin_tones.Add(new Color32(216, 133, 106, 255)); // Tan
        skin_tones.Add(new Color32(201, 107, 78, 255)); // Bronze
        skin_tones.Add(new Color32(185, 81, 54, 255)); // Sienna
        skin_tones.Add(new Color32(167, 54, 32, 255)); // Walnut
        skin_tones.Add(new Color32(161, 92, 40, 255)); // Golden
        skin_tones.Add(new Color32(139, 71, 25, 255)); // Amber
        skin_tones.Add(new Color32(122, 44, 6, 255)); // Cinnamon      
        skin_tones.Add(new Color32(70, 37, 13, 255)); // Coffee
        skin_tones.Add(new Color32(41, 21, 6, 255)); // Dark
        skin_tones.Insert(2, new Color32(255, 219, 172, 255)); // Asian
        skin_tones.Insert(23, new Color32(78, 47, 22, 255)); // African
    }

    private void InitializeLipsColors()
    {
        lips_colors = new List<Color32>();

        // Natural lips colors (60%)
        lips_colors.Add(new Color32(255, 226, 213, 255)); // Pale Pink
        lips_colors.Add(new Color32(245, 195, 186, 255)); // Beige Pink
        lips_colors.Add(new Color32(216, 136, 130, 255)); // Nude Rose
        lips_colors.Add(new Color32(216, 110, 96, 255)); // Muted Red
        lips_colors.Add(new Color32(194, 85, 75, 255)); // Tawny
        lips_colors.Add(new Color32(181, 63, 51, 255)); // Rust
        lips_colors.Add(new Color32(194, 68, 51, 255)); // Burnt Orange
        lips_colors.Add(new Color32(159, 37, 33, 255)); // Sienna Red
        lips_colors.Add(new Color32(131, 28, 24, 255)); // Burgundy           
        lips_colors.Add(new Color32(181, 49, 30, 255)); // Brick Red
        lips_colors.Add(new Color32(159, 30, 12, 255)); // Cinnamon Brown
        lips_colors.Add(new Color32(97, 14, 10, 255)); // Cherry Red 
        lips_colors.Add(new Color32(60, 27, 3, 255)); // Coffee
        lips_colors.Add(new Color32(31, 11, 1, 255)); // Dark
        lips_colors.Add(new Color32(0, 0, 0, 255)); // Black

        lips_colors.Add(new Color32(210, 0, 0, 255)); // Bright Red
        lips_colors.Add(new Color32(197, 25, 88, 255)); // Deep Pink
        lips_colors.Add(new Color32(221, 102, 10, 255)); // Orange
        lips_colors.Add(new Color32(54, 94, 37, 255)); // Green
        lips_colors.Add(new Color32(125, 217, 228, 255)); // Light blue
        lips_colors.Add(new Color32(14, 141, 211, 255)); // Sky Blue
        lips_colors.Add(new Color32(71, 52, 158, 255)); // Blue Violet
        lips_colors.Add(new Color32(125, 55, 167, 255)); // Purple
        lips_colors.Add(new Color32(158, 58, 111, 255)); // Violet
        lips_colors.Add(new Color32(188, 63, 25, 255)); // Coral
        lips_colors.Add(new Color32(222, 157, 63, 255)); // Gold
    }

    private void InitializeEyeColors()
    {
        eye_colors = new List<Color32>();

        eye_colors.Add(new Color32(48, 31, 12, 255)); // Dark Brown
        eye_colors.Add(new Color32(82, 51, 20, 255)); // Brown
        eye_colors.Add(new Color32(124, 77, 29, 255)); // Light Brown
        eye_colors.Add(new Color32(176, 108, 41, 255)); // Hazel-Brown
        eye_colors.Add(new Color32(219, 151, 61, 255)); // Amber
        eye_colors.Add(new Color32(252, 194, 115, 255)); // Light Amber
        eye_colors.Add(new Color32(98, 69, 41, 255)); // Dark Amber
        eye_colors.Add(new Color32(70, 89, 117, 255)); // Blue-Gray
        eye_colors.Add(new Color32(128, 152, 179, 255)); // Light Blue-Gray
        eye_colors.Add(new Color32(166, 188, 211, 255)); // Pale Blue-Gray
        eye_colors.Add(new Color32(197, 223, 247, 255)); // Very Pale Blue-Gray
        eye_colors.Add(new Color32(54, 45, 36, 255)); // Dark Gray-Brown
        eye_colors.Add(new Color32(98, 82, 66, 255)); // Gray-Brown
        eye_colors.Add(new Color32(139, 118, 92, 255)); // Light Gray-Brown
        eye_colors.Add(new Color32(185, 161, 131, 255)); // Pale Gray-Brown
        eye_colors.Add(new Color32(211, 189, 157, 255)); // Very Pale Gray-Brown
        eye_colors.Add(new Color32(29, 69, 54, 255)); // Dark Green
        eye_colors.Add(new Color32(69, 110, 93, 255)); // Green
        eye_colors.Add(new Color32(124, 167, 146, 255)); // Light Green
        eye_colors.Add(new Color32(164, 205, 186, 255)); // Pale Green
        eye_colors.Add(new Color32(75, 57, 27, 255)); // Dark Hazel-Brown
        eye_colors.Add(new Color32(119, 92, 45, 255)); // Hazel-Brown
        eye_colors.Add(new Color32(165, 128, 63, 255)); // Light Hazel-Brown
        eye_colors.Add(new Color32(207, 169, 96, 255)); // Pale Hazel-Brown


        eye_colors.Insert(7, new Color32(51, 55, 93, 255)); // Dark Blue
        eye_colors.Insert(0, new Color32(46, 13, 3, 255)); // Dark Chocolate
    }

    private void InitializeHairColors()
    {
        hair_colors = new List<Color32>();

        hair_colors.Add(new Color32(30, 30, 30, 255)); // Black
        hair_colors.Add(new Color32(80, 80, 80, 255)); // Dark Gray
        hair_colors.Add(new Color32(122, 122, 122, 255)); // Gray
        hair_colors.Add(new Color32(200, 200, 200, 255)); // White
        hair_colors.Add(new Color32(147, 128, 108, 255)); // Brown-Gray
        hair_colors.Add(new Color32(51, 26, 0, 255)); // Light Brown
        hair_colors.Add(new Color32(72, 24, 0, 255)); // Dark Chocolate
        hair_colors.Add(new Color32(102, 51, 0, 255)); // Medium Blonde
        hair_colors.Add(new Color32(99, 46, 0, 255)); // Dark Brown
        hair_colors.Add(new Color32(133, 71, 0, 255)); // Espresso

        hair_colors.Add(new Color32(129, 86, 65, 255)); // Brown Black
        hair_colors.Add(new Color32(128, 32, 0, 255)); // Medium Blonde
        hair_colors.Add(new Color32(179, 45, 0, 255)); // Light Blonde
        hair_colors.Add(new Color32(206, 91, 28, 255)); // Very Light Blonde
        hair_colors.Add(new Color32(205, 116, 73, 255)); // Dark Orange-Red
        hair_colors.Add(new Color32(224, 137, 84, 255)); // Medium Orange-Red
        hair_colors.Add(new Color32(239, 165, 106, 255)); // Light Orange-Red
        hair_colors.Add(new Color32(191, 162, 105, 255)); // Medium Brown
        hair_colors.Add(new Color32(175, 136, 60, 255)); // Dark Brown
        hair_colors.Add(new Color32(228, 208, 163, 255)); // Dark Blonde

        hair_colors.Add(new Color32(246, 185, 202, 255)); // Pastel Pink
        hair_colors.Add(new Color32(190, 232, 211, 255)); // Pastel Green
        hair_colors.Add(new Color32(128, 255, 128, 255)); // Bright Green
        hair_colors.Add(new Color32(128, 128, 255, 255)); // Bright Blue
        hair_colors.Add(new Color32(255, 128, 255, 255)); // Bright Purple
        hair_colors.Add(new Color32(230, 0, 0, 255)); // Pastel Purple
        hair_colors.Add(new Color32(0, 0, 179, 255)); // Bright Green
        hair_colors.Add(new Color32(51, 204, 255, 255)); // Bright Blue
        hair_colors.Add(new Color32(153, 51, 255, 255)); // Pastel Purple
        hair_colors.Add(new Color32(204, 51, 153, 255)); // Bright Purple
    }

    private void InitializeGlassesColors()
    {
        glasses_colors = new List<Color32>();

        glasses_colors.Add(new Color32(27, 27, 27, 255));    // Black
        glasses_colors.Add(new Color32(99, 99, 99, 255));    // Gray
        glasses_colors.Add(new Color32(200, 200, 200, 255)); // White
        glasses_colors.Add(new Color32(48, 31, 12, 255)); // Dark Brown
        glasses_colors.Add(new Color32(82, 51, 20, 255)); // Brown
        glasses_colors.Add(new Color32(124, 77, 29, 255)); // Light Brown
        glasses_colors.Add(new Color32(194, 143, 107, 255)); // Chestnut
        glasses_colors.Add(new Color32(139, 71, 25, 255)); // Amber
        glasses_colors.Add(new Color32(12, 16, 57, 255)); // Very dark blue
        glasses_colors.Add(new Color32(25, 52, 115, 255));   // Dark blue
        glasses_colors.Add(new Color32(97, 14, 10, 255)); // Cherry Red 
        glasses_colors.Add(new Color32(159, 30, 12, 255)); // Cinnamon Brown
        glasses_colors.Add(new Color32(43, 96, 47, 255));   // Dark Green
    }

    private void ButtonsGenerator(int buttonNumber, int nCol, float multiplier, float buttonDim, int xOffset, int yOffset, List<Color32> colorList, 
        Transform panel, string materialName, List<SkinnedMeshRenderer> meshList, string key)
    {
        materialName += " (Instance)";
        for (int i = 0; i < buttonNumber; i++)
        {
            // Create a new button from the prefab
            GameObject buttonObject = Instantiate(prefabButton, panel);

            // Set the button's position and size
            int row = i / nCol;
            int col = i % nCol;
            Vector2 buttonPosition = new Vector2(col * buttonDim* multiplier -xOffset, -row * buttonDim + yOffset);
            buttonObject.GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            buttonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonDim * 1.2f, buttonDim);

            buttonObject.GetComponent<Image>().color = colorList[i];

            // Change color when button clicked
            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {

            foreach (SkinnedMeshRenderer skin in meshList)
                {
                    foreach (Material m in skin.materials)
                    {
                        if (m.name.Equals(materialName))
                        {
                            m.color = buttonObject.GetComponent<Image>().color;
                            AvatarSettings.Instance.SetColor(key, buttonObject.GetComponent<Image>().color);
                        }
                    }
                }
            if(materialName.Equals("Hair (Instance)"))
                {
                    foreach (SkinnedMeshRenderer skin in eyebrowsList)
                    {
                        foreach(Material m in skin.materials)
                        {
                            m.color = buttonObject.GetComponent<Image>().color;
                        }
                    }
                    foreach (SkinnedMeshRenderer skin in beardsList)
                    {
                        foreach (Material m in skin.materials)
                        {
                            m.color = buttonObject.GetComponent<Image>().color;
                        }
                    }
                }
            });
        }
    }
    public int ChangeMesh(List<SkinnedMeshRenderer> list, int selected, int value, TMP_Text indicator)
    {
        selected += value;

        if(selected >= list.Count)
        {
            selected = 0;
        }

        if (selected < 0)
        {
            selected = list.Count - 1;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (i == selected)
                list[i].enabled = true;
            else list[i].enabled = false;
        }
        indicator.text = (selected + 1).ToString();
        return selected;
    }
}

