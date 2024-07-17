using UnityEngine;
using UnityEngine.UI;

public class Lith9Hand : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public Lithology9 lithology9; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (lithology9.lithology9pop.activeSelf)
        {
            lithology9.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            lithology9.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
