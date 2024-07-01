using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler2 : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public MenuManager3 menuManager3; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (menuManager3.objectivePopUp.activeSelf)
        {
            menuManager3.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            menuManager3.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
