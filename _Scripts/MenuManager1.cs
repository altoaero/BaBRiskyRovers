using UnityEngine;

public class MenuManager1 : MonoBehaviour
{
    public GameObject popUpMenu2; // Reference to the pop-up menu panel for Menu1

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        popUpMenu2.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        popUpMenu2.SetActive(false);
    }
}
