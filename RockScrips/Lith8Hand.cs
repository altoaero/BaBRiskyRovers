using UnityEngine;
using UnityEngine.UI;

public class Lith8Hand : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public Lithology8 lithology8; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (lithology8.lithology8pop.activeSelf)
        {
            lithology8.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            lithology8.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
