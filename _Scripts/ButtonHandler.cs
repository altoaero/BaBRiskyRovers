using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public MenuManager menuManager; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (menuManager.popUpMenu.activeSelf)
        {
            menuManager.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            menuManager.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
