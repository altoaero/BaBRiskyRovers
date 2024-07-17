using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataLogsButtonHandler : MonoBehaviour
{
    public Button myButton; // Reference to the Button
    public DataLogsMenuManager dataLogsMenuManager; // Reference to the MenuManager

    // Start is called before the first frame update
    void Start()
    {
        // Register the button click event
        myButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        Debug.Log("Button Clicked!");
        if (dataLogsMenuManager.dataLogsPopUp.activeSelf)
        {
            dataLogsMenuManager.HideMenu(); // Hide the menu if it's currently active
        }
        else
        {
            dataLogsMenuManager.ShowMenu(); // Show the menu if it's currently hidden
        }
    }
}
