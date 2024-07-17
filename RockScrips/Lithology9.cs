using UnityEngine;

public class Lithology9 : MonoBehaviour
{
    public GameObject lithology9pop; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        lithology9pop.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        lithology9pop.SetActive(false);
    }
}
