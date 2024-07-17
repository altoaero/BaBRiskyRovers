using UnityEngine;

public class Lithology13 : MonoBehaviour
{
    public GameObject lithology13pop; // Reference to the pop-up menu panel

    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        lithology13pop.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        lithology13pop.SetActive(false);
    }
}
