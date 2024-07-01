using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHelp : MonoBehaviour
{
    private GameManagerScore gameManagerScore;
    private bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScore = GameObject.Find("GameManagerScore").GetComponent<GameManagerScore>();
        
    }

    private void OnMouseDown() {
        if (stop) {
            return;
        }
        gameManagerScore.UpdateScore(1);
        stop = true;

    } 
}