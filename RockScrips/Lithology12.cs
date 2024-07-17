using UnityEngine;

public class Lithology12 : MonoBehaviour
{
    public GameObject lithology12pop; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        lithology12pop.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        lithology12pop.SetActive(false);
    }
}
