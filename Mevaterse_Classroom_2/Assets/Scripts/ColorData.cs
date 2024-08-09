using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorData : MonoBehaviour
{
    private List<Color32> uniformColors,
                tieColors,
                skinTones,
                lipsColors,
                eyeColors,
                hairColors,
                glassesColors;

    void Start()
    {
        InitializeEyeColors();
        InitializeGlassesColors();
        InitializeHairColors();
        InitializeLipsColors();
        InitializeSkinTones();
        InitializeTieColors();
        InitializeUniformColors();
    }

    public List<Color32> GetUniformColors()
    {
        return uniformColors;
    }

    public List<Color32> GetTieColors()
    {
        return tieColors;
    }

    public List<Color32> GetSkinTones()
    {
        return skinTones;
    }

    public List<Color32> GetLipsColors()
    {
        return lipsColors;
    }

    public List<Color32> GetEyeColors()
    {
        return eyeColors;
    }

    public List<Color32> GetHairColors()
    {
        return hairColors;
    }

    public List<Color32> GetGlassesColors()
    {
        return glassesColors;
    }
    
    private void InitializeUniformColors()
    {
        uniformColors = new List<Color32>();

        uniformColors.Add(new Color32(27, 27, 27, 255));    // Black
        uniformColors.Add(new Color32(99, 99, 99, 255));    // Gray
        uniformColors.Add(new Color32(200, 200, 200, 255)); // White
        uniformColors.Add(new Color32(12, 16, 57, 255)); // Very dark blue
        uniformColors.Add(new Color32(25, 52, 115, 255));   // Dark blue
        uniformColors.Add(new Color32(91, 134, 229, 255));  // Light blue
        uniformColors.Add(new Color32(43, 96, 47, 255));   // Dark Green
        uniformColors.Add(new Color32(68, 159, 70, 255));   // Green
        uniformColors.Add(new Color32(0, 137, 123, 255)); // Aquamarine
        uniformColors.Add(new Color32(230, 165, 45, 255));  // Orange
        uniformColors.Add(new Color32(244, 147, 180, 255));  // Pink
        uniformColors.Add(new Color32(177, 60, 60, 255));   // Red
        uniformColors.Add(new Color32(123, 39, 39, 255));   // Dark red
        uniformColors.Add(new Color32(139, 69, 19, 255));   // Brown 

    }
    private void InitializeTieColors()
    {
        tieColors = new List<Color32>();

        tieColors.Add(new Color32(110, 166, 66, 255));   // Bright green
        tieColors.Add(new Color32(32, 115, 41, 255));    // Forest green
        tieColors.Add(new Color32(34, 42, 119, 255));   // Dark Blue
        tieColors.Add(new Color32(20, 94, 179, 255));   // Blue
        tieColors.Add(new Color32(85, 160, 186, 255));   // Sky blue
        tieColors.Add(new Color32(120, 62, 136, 255));   // Purple
        tieColors.Add(new Color32(227, 111, 163, 255));  // Pink
        tieColors.Add(new Color32(235, 100, 0, 255));    // Orange-red
        tieColors.Add(new Color32(235, 78, 0, 255));     // Bright orange
        tieColors.Add(new Color32(190, 0, 0, 255));      // Red
        tieColors.Add(new Color32(131, 21, 21, 255));    // Dark red
        tieColors.Add(new Color32(27, 27, 27, 255)); // Black
        tieColors.Add(new Color32(99, 99, 99, 255));    // Gray
        tieColors.Add(new Color32(255, 255, 255, 255)); // White

    }

    private void InitializeSkinTones()
    {
        skinTones = new List<Color32>();

        skinTones.Add(new Color32(255, 240, 215, 255)); // Ivory
        skinTones.Add(new Color32(245, 221, 203, 255)); // Fair
        skinTones.Add(new Color32(229, 195, 166, 255)); // Porcelain
        skinTones.Add(new Color32(209, 161, 129, 255)); // Light Beige   
        skinTones.Add(new Color32(215, 169, 140, 255)); // Almond
        skinTones.Add(new Color32(194, 145, 98, 255)); // Nude
        skinTones.Add(new Color32(194, 143, 107, 255)); // Chestnut
        skinTones.Add(new Color32(169, 118, 76, 255)); // Cocoa
        skinTones.Add(new Color32(181, 118, 67, 255)); // Honey
        skinTones.Add(new Color32(143, 91, 42, 255)); // Espresso
        skinTones.Add(new Color32(114, 67, 34, 255)); // Mocha
        skinTones.Add(new Color32(99, 58, 30, 255)); // Sable
        skinTones.Add(new Color32(255, 223, 196, 255)); // Peach
        skinTones.Add(new Color32(249, 192, 169, 255)); // Rose
        skinTones.Add(new Color32(238, 163, 144, 255)); // Sand        
        skinTones.Add(new Color32(216, 133, 106, 255)); // Tan
        skinTones.Add(new Color32(201, 107, 78, 255)); // Bronze
        skinTones.Add(new Color32(185, 81, 54, 255)); // Sienna
        skinTones.Add(new Color32(167, 54, 32, 255)); // Walnut
        skinTones.Add(new Color32(161, 92, 40, 255)); // Golden
        skinTones.Add(new Color32(139, 71, 25, 255)); // Amber
        skinTones.Add(new Color32(122, 44, 6, 255)); // Cinnamon      
        skinTones.Add(new Color32(70, 37, 13, 255)); // Coffee
        skinTones.Add(new Color32(41, 21, 6, 255)); // Dark
        skinTones.Insert(2, new Color32(255, 219, 172, 255)); // Asian
        skinTones.Insert(23, new Color32(78, 47, 22, 255)); // African
    }

    private void InitializeLipsColors()
    {
        lipsColors = new List<Color32>();

        // Natural lips colors (60%)
        lipsColors.Add(new Color32(255, 226, 213, 255)); // Pale Pink
        lipsColors.Add(new Color32(245, 195, 186, 255)); // Beige Pink
        lipsColors.Add(new Color32(216, 136, 130, 255)); // Nude Rose
        lipsColors.Add(new Color32(216, 110, 96, 255)); // Muted Red
        lipsColors.Add(new Color32(194, 85, 75, 255)); // Tawny
        lipsColors.Add(new Color32(181, 63, 51, 255)); // Rust
        lipsColors.Add(new Color32(194, 68, 51, 255)); // Burnt Orange
        lipsColors.Add(new Color32(159, 37, 33, 255)); // Sienna Red
        lipsColors.Add(new Color32(131, 28, 24, 255)); // Burgundy           
        lipsColors.Add(new Color32(181, 49, 30, 255)); // Brick Red
        lipsColors.Add(new Color32(159, 30, 12, 255)); // Cinnamon Brown
        lipsColors.Add(new Color32(97, 14, 10, 255)); // Cherry Red 
        lipsColors.Add(new Color32(60, 27, 3, 255)); // Coffee
        lipsColors.Add(new Color32(31, 11, 1, 255)); // Dark
        lipsColors.Add(new Color32(0, 0, 0, 255)); // Black

        lipsColors.Add(new Color32(210, 0, 0, 255)); // Bright Red
        lipsColors.Add(new Color32(197, 25, 88, 255)); // Deep Pink
        lipsColors.Add(new Color32(221, 102, 10, 255)); // Orange
        lipsColors.Add(new Color32(54, 94, 37, 255)); // Green
        lipsColors.Add(new Color32(125, 217, 228, 255)); // Light blue
        lipsColors.Add(new Color32(14, 141, 211, 255)); // Sky Blue
        lipsColors.Add(new Color32(71, 52, 158, 255)); // Blue Violet
        lipsColors.Add(new Color32(125, 55, 167, 255)); // Purple
        lipsColors.Add(new Color32(158, 58, 111, 255)); // Violet
        lipsColors.Add(new Color32(188, 63, 25, 255)); // Coral
        lipsColors.Add(new Color32(222, 157, 63, 255)); // Gold
    }

    private void InitializeEyeColors()
    {
        eyeColors = new List<Color32>();

        eyeColors.Add(new Color32(48, 31, 12, 255)); // Dark Brown
        eyeColors.Add(new Color32(82, 51, 20, 255)); // Brown
        eyeColors.Add(new Color32(124, 77, 29, 255)); // Light Brown
        eyeColors.Add(new Color32(176, 108, 41, 255)); // Hazel-Brown
        eyeColors.Add(new Color32(219, 151, 61, 255)); // Amber
        eyeColors.Add(new Color32(252, 194, 115, 255)); // Light Amber
        eyeColors.Add(new Color32(98, 69, 41, 255)); // Dark Amber
        eyeColors.Add(new Color32(70, 89, 117, 255)); // Blue-Gray
        eyeColors.Add(new Color32(128, 152, 179, 255)); // Light Blue-Gray
        eyeColors.Add(new Color32(166, 188, 211, 255)); // Pale Blue-Gray
        eyeColors.Add(new Color32(197, 223, 247, 255)); // Very Pale Blue-Gray
        eyeColors.Add(new Color32(54, 45, 36, 255)); // Dark Gray-Brown
        eyeColors.Add(new Color32(98, 82, 66, 255)); // Gray-Brown
        eyeColors.Add(new Color32(139, 118, 92, 255)); // Light Gray-Brown
        eyeColors.Add(new Color32(185, 161, 131, 255)); // Pale Gray-Brown
        eyeColors.Add(new Color32(211, 189, 157, 255)); // Very Pale Gray-Brown
        eyeColors.Add(new Color32(29, 69, 54, 255)); // Dark Green
        eyeColors.Add(new Color32(69, 110, 93, 255)); // Green
        eyeColors.Add(new Color32(124, 167, 146, 255)); // Light Green
        eyeColors.Add(new Color32(164, 205, 186, 255)); // Pale Green
        eyeColors.Add(new Color32(75, 57, 27, 255)); // Dark Hazel-Brown
        eyeColors.Add(new Color32(119, 92, 45, 255)); // Hazel-Brown
        eyeColors.Add(new Color32(165, 128, 63, 255)); // Light Hazel-Brown
        eyeColors.Add(new Color32(207, 169, 96, 255)); // Pale Hazel-Brown


        eyeColors.Insert(7, new Color32(51, 55, 93, 255)); // Dark Blue
        eyeColors.Insert(0, new Color32(46, 13, 3, 255)); // Dark Chocolate
    }

    private void InitializeHairColors()
    {
        hairColors = new List<Color32>();

        hairColors.Add(new Color32(30, 30, 30, 255)); // Black
        hairColors.Add(new Color32(80, 80, 80, 255)); // Dark Gray
        hairColors.Add(new Color32(122, 122, 122, 255)); // Gray
        hairColors.Add(new Color32(200, 200, 200, 255)); // White
        hairColors.Add(new Color32(147, 128, 108, 255)); // Brown-Gray
        hairColors.Add(new Color32(51, 26, 0, 255)); // Light Brown
        hairColors.Add(new Color32(72, 24, 0, 255)); // Dark Chocolate
        hairColors.Add(new Color32(102, 51, 0, 255)); // Medium Blonde
        hairColors.Add(new Color32(99, 46, 0, 255)); // Dark Brown
        hairColors.Add(new Color32(133, 71, 0, 255)); // Espresso

        hairColors.Add(new Color32(129, 86, 65, 255)); // Brown Black
        hairColors.Add(new Color32(128, 32, 0, 255)); // Medium Blonde
        hairColors.Add(new Color32(179, 45, 0, 255)); // Light Blonde
        hairColors.Add(new Color32(206, 91, 28, 255)); // Very Light Blonde
        hairColors.Add(new Color32(205, 116, 73, 255)); // Dark Orange-Red
        hairColors.Add(new Color32(224, 137, 84, 255)); // Medium Orange-Red
        hairColors.Add(new Color32(239, 165, 106, 255)); // Light Orange-Red
        hairColors.Add(new Color32(191, 162, 105, 255)); // Medium Brown
        hairColors.Add(new Color32(175, 136, 60, 255)); // Dark Brown
        hairColors.Add(new Color32(228, 208, 163, 255)); // Dark Blonde

        hairColors.Add(new Color32(246, 185, 202, 255)); // Pastel Pink
        hairColors.Add(new Color32(190, 232, 211, 255)); // Pastel Green
        hairColors.Add(new Color32(128, 255, 128, 255)); // Bright Green
        hairColors.Add(new Color32(128, 128, 255, 255)); // Bright Blue
        hairColors.Add(new Color32(255, 128, 255, 255)); // Bright Purple
        hairColors.Add(new Color32(230, 0, 0, 255)); // Pastel Purple
        hairColors.Add(new Color32(0, 0, 179, 255)); // Bright Green
        hairColors.Add(new Color32(51, 204, 255, 255)); // Bright Blue
        hairColors.Add(new Color32(153, 51, 255, 255)); // Pastel Purple
        hairColors.Add(new Color32(204, 51, 153, 255)); // Bright Purple
    }

    private void InitializeGlassesColors()
    {
        glassesColors = new List<Color32>();

        glassesColors.Add(new Color32(27, 27, 27, 255));    // Black
        glassesColors.Add(new Color32(99, 99, 99, 255));    // Gray
        glassesColors.Add(new Color32(200, 200, 200, 255)); // White
        glassesColors.Add(new Color32(48, 31, 12, 255)); // Dark Brown
        glassesColors.Add(new Color32(82, 51, 20, 255)); // Brown
        glassesColors.Add(new Color32(124, 77, 29, 255)); // Light Brown
        glassesColors.Add(new Color32(194, 143, 107, 255)); // Chestnut
        glassesColors.Add(new Color32(139, 71, 25, 255)); // Amber
        glassesColors.Add(new Color32(12, 16, 57, 255)); // Very dark blue
        glassesColors.Add(new Color32(25, 52, 115, 255));   // Dark blue
        glassesColors.Add(new Color32(97, 14, 10, 255)); // Cherry Red 
        glassesColors.Add(new Color32(159, 30, 12, 255)); // Cinnamon Brown
        glassesColors.Add(new Color32(43, 96, 47, 255));   // Dark Green
    }

}
