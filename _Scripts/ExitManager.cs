using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public GameObject areYouSure; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        areYouSure.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        areYouSure.SetActive(false);
    }
}
