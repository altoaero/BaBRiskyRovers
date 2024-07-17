using UnityEngine;
using UnityEngine.UI;

public class ExitHand : MonoBehaviour
{
    public Button myButton; // Reference to the Button that toggles the menu
    public Button closeButton; // Reference to the Button that closes the menu
    public ExitManager exitManager; // Reference to the ExitManager

    void Start()
    {
        // Register the button click events
        myButton.onClick.AddListener(ToggleMenu);
        closeButton.onClick.AddListener(CloseMenu);
    }

    void ToggleMenu()
    {
        if (exitManager.areYouSure.activeSelf)
        {
            exitManager.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            exitManager.ShowMenu(); // Show the menu if it's currently hidden
        }
    }

    void CloseMenu()
    {
        if (exitManager.areYouSure.activeSelf)
        {
            exitManager.HideMenu(); // Hide the menu if it's currently active
        }
    }
}
