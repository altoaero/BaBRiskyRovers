using UnityEngine;

public class Lithology8 : MonoBehaviour
{
    public GameObject lithology8pop; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        lithology8pop.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        lithology8pop.SetActive(false);
    }
}
