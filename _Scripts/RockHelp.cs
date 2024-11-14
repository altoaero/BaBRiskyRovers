using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHelp : MonoBehaviour
{
    private GameManagerScore gameManagerScore; // the object connecting to gamemanagerscore script
    private bool stop = false; // checks if rock has been clicked 
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScore = GameObject.Find("Score").GetComponent<GameManagerScore>(); // finds the score object in heirarchy
        
    }

    private void OnMouseDown() {
        //Debug.Log("clicked?");
        if (stop) { // if rock has been clicked 
            return;
        }
        gameManagerScore.UpdateScore(1); // updates score 
        stop = true;

    } 
}
