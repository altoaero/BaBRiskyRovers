using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLogsMenuManager : MonoBehaviour
{
    public GameObject dataLogsPopUp; // Reference to the pop-up menu panel

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the menu is hidden at the start
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        dataLogsPopUp.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        dataLogsPopUp.SetActive(false);
    }
}
