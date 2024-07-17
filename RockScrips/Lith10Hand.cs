using UnityEngine;
using UnityEngine.UI;

public class Lith10Hand : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public Lithology10 lithology10; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (lithology10.lithology10pop.activeSelf)
        {
            lithology10.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            lithology10.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
