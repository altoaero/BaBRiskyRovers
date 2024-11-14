using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScore : MonoBehaviour
{

    public TextMeshProUGUI mainText; // score text in the heirarchy
    public int score = 0;
   

    void Start() {
        mainText.text = "Rocks Found: " + score + "/6"; // starting line when game starts 
    }
    public void UpdateScore(int s) {
        score += s;
        mainText.text = "Rocks Found: " + score + "/6"; // sets score to +1
    }

}


