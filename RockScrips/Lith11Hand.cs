using UnityEngine;
using UnityEngine.UI;

public class Lith11Hand : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public Lithology11 lithology11; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (lithology11.lithology11pop.activeSelf)
        {
            lithology11.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            lithology11.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
