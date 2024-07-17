using UnityEngine;
using UnityEngine.UI;

public class Lith12Hand : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public Lithology12 lithology12; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (lithology12.lithology12pop.activeSelf)
        {
            lithology12.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            lithology12.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
