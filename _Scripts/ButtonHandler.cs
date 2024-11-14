using UnityEngine; 
using UnityEngine.UI; 

public class ButtonHandler : MonoBehaviour
{
    public Button myButton; // Public variable to hold a reference to the Button component
    public MenuManager menuManager; // Public variable to hold a reference to the MenuManager script

    void Start()
    {
        // Register the ToggleMenu method to be called when the button is clicked
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!"); // Log a message to the console for debugging purposes

        // Check if the pop-up menu is currently active
        if (menuManager.popUpMenu.activeSelf)
        {
            menuManager.HideMenu(); // Call the HideMenu method if the menu is active
        }
        else
        {
            menuManager.ShowMenu(); // Call the ShowMenu method if the menu is inactive
        }
    }
}
