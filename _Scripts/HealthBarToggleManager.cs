using UnityEngine;
using UnityEngine.UI;

public class HealthBarToggleManager : MonoBehaviour
{
    public Slider movementHealthSlider; // Reference to the movement health slider
    public Slider collisionHealthSlider; // Reference to the collision health slider
    public Button toggleButton; // Reference to the toggle button
    private Text toggleButtonText; // Reference to the Text component of the button

    private bool areSmallHealthBarsVisible = false;

    void Start()
    {
        // Ensure the small health bars are initially hidden
        movementHealthSlider.gameObject.SetActive(false);
        collisionHealthSlider.gameObject.SetActive(false);

        // Get the Text component of the button
        toggleButtonText = toggleButton.GetComponentInChildren<Text>();

        // Add a listener to the toggle button
        toggleButton.onClick.AddListener(ToggleSmallHealthBars);
    }

    void ToggleSmallHealthBars()
    {
        areSmallHealthBarsVisible = !areSmallHealthBarsVisible;
        movementHealthSlider.gameObject.SetActive(areSmallHealthBarsVisible);
        collisionHealthSlider.gameObject.SetActive(areSmallHealthBarsVisible);

       
    }
}
