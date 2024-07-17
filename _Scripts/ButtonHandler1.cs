using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler1 : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public MenuManager1 menuManager1; // Reference to the MenuManager

    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (menuManager1.popUpMenu2.activeSelf)
        {
            menuManager1.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            menuManager1.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
