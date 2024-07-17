using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject popUpMenu; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        popUpMenu.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        popUpMenu.SetActive(false);
    }
}
