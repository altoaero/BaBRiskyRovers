using UnityEngine;

public class Lithology10 : MonoBehaviour
{
    public GameObject lithology10pop; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        lithology10pop.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        lithology10pop.SetActive(false);
    }
}
