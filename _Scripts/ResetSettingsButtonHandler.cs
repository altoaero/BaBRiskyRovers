using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResetSettingsButtonHandler : MonoBehaviour
{
    public Button myButton; // Reference to the reset Button
    public TMP_Dropdown dropdown; // Reference to the music type dropdown
    public Slider FOVSlider; // Reference to the FOV slider
    public Slider VolumeSlider; // Reference to the volumer slider

    // Start is called before the first frame update
    void Start()
    {
        // Set the default music to none
        dropdown.value = 0;
        
        // Set the default FOV to 60
        FOVSlider.value = 0.5f;

        // Set the default volume to 100
        VolumeSlider.value = 1.0f;
        
        // Register the button click event
        myButton.onClick.AddListener(ResetSettings);
    }

    void ResetSettings() {
        // Set the default music to none
        dropdown.value = 0;
        
        // Set the default FOV to 60
        FOVSlider.value = 0.5f;

        // Set the default volume to 100
        VolumeSlider.value = 1.0f;
    }
}