using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockexperiment : MonoBehaviour
{
    public int rockNum;
    private ExperimentImage experimentButton;


    void Start() {
        experimentButton = GameObject.Find("ExperimentImage").GetComponent<ExperimentImage>();
    }
    private void OnTriggerEnter2D(Collider2D x) {

        
        experimentButton.ChangeImage(rockNum);
    }

    private void OnTriggerExit2D(Collider2D y) {
        experimentButton.ResetImage();
    }

    
}
