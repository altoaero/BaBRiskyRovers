using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI mainText;
    public int score = 0;
   

    void Start() {
        mainText.text = "Rocks Found: " + score + "/4";
    }
    public void UpdateScore(int s) {
        score += s;
        mainText.text = "Rocks Found: " + score + "/4";
    }
}

