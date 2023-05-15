using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public TMP_Dropdown screenDrop;
    public Slider micSlider;
    public Slider bgSlider;

    public Recorder recorder;
    public AudioSource bgnoise;

    public TMP_Text note;


    private void Start()
    {
        UpdateNote();

        micSlider.value = recorder.VoiceDetectionThreshold;
        bgSlider.value = bgnoise.volume;

        screenDrop.onValueChanged.AddListener(OnWindowModeChanged);
        bgSlider.onValueChanged.AddListener(OnBgSliderChanged);
        bgSlider.onValueChanged.AddListener(OnMicSliderChanged);
    }

    private void LateUpdate()
    {
        UpdateNote();
    }

    void OnWindowModeChanged(int index)
    {
        // Get the selected option from the dropdown
        string selectedOption = screenDrop.options[index].text;

        // Change the game window mode based on the selected option
        if (selectedOption == "Full Screen")
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (selectedOption == "Windowed")
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    void UpdateNote()
    {
        if(Microphone.devices.Length == 0)
            note.text = $"Note: Photon Voice does not allow to change the microphone used directly in the application. If you want to choose a difference input device, please go to your device audio settings. " +
            $"\nYour current detected device is: <color=yellow>none</color>";
        else
            note.text = $"Note: Photon Voice does not allow to change the microphone used directly in the application. If you want to choose a difference input device, please go to your device audio settings. " +
            $"\nYour current detected device is: <color=yellow>{Microphone.devices[0]}</color>";
    }

    void OnBgSliderChanged(float value)
    {
        bgnoise.volume = value;
    }
    void OnMicSliderChanged(float value)
    {
        recorder.VoiceDetectionThreshold = value;
    }

}
