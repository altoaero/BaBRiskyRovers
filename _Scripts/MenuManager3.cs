using UnityEngine;

public class MenuManager3 : MonoBehaviour
{
    public GameObject objectivePopUp; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        objectivePopUp.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        objectivePopUp.SetActive(false);
    }
}
