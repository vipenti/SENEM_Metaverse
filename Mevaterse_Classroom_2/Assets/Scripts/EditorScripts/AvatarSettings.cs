using UnityEngine;
using Vipenti.Singletons;
using ExitGames.Client.Photon;

public class AvatarSettings : Singleton<AvatarSettings>
{
    public Hashtable colorSettings;

    public int selectedUniform;
    public int selectedHaircut;
    public int selectedEyebrows;
    public int selectedLashes;
    public int selectedGlasses;
    public int selectedBeard;
    

    void Awake()
    {
        DontDestroyOnLoad(this);
        colorSettings = new Hashtable();

        colorSettings.Add("skin", new Color32(217, 177, 158, 255));
        colorSettings.Add("eyes", new Color32(34, 65, 12, 94));
        colorSettings.Add("hair", new Color32(32, 18, 3, 94));
        colorSettings.Add("uniform", new Color32(3, 14, 52, 89));
        colorSettings.Add("shirt", new Color32(149, 149, 149, 2));
        colorSettings.Add("tie", new Color32(118, 0, 0, 87));
        colorSettings.Add("lips", new Color32(255, 192, 172, 255));
        colorSettings.Add("glasses", new Color32(23, 23, 23, 255));

        selectedHaircut = 0;
        selectedUniform = 0;
        selectedHaircut= 0;
        selectedEyebrows = 0;
        selectedLashes = 0;
        selectedGlasses = 0;
        selectedBeard = 0;
}

    public Color32 GetColorByKey(string key)
    {
        return (Color32)colorSettings[key];
        
    }

    public void SetColor(string key, Color32 color)
    {
        colorSettings[key] = color;
    }
}
