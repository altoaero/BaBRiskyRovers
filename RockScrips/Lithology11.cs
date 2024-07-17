using UnityEngine;

public class Lithology11 : MonoBehaviour
{
    public GameObject lithology11pop; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        lithology11pop.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        lithology11pop.SetActive(false);
    }
}
