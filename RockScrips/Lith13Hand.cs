using UnityEngine;
using UnityEngine.UI;

public class Lith13Hand : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public Lithology13 lithology13; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (lithology13.lithology13pop.activeSelf)
        {
            lithology13.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            lithology13.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
