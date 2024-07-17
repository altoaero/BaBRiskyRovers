using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RockPopUp : MonoBehaviour
{

    public GameObject rockPopUp;
    // Start is called before the first frame update
    void Start()
    {
        HideMenu();
    }

    public void ShowMenu()
    {
        // Show the menu when called
        rockPopUp.SetActive(true);
    }

    public void HideMenu()
    {
        // Hide the menu
        rockPopUp.SetActive(false);
    }

}
