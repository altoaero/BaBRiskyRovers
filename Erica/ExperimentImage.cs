using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperimentImage : MonoBehaviour
{
    public GameObject[] rockList;
    //public Sprite rock5;
    public GameObject defaultImage;
    //public Image dataImage;
    public bool activeS;

    void Start() {
        ResetImage();
    }

    public void ChangeImage(int num) {

        defaultImage.SetActive(false);
        for(int i = 0; i < rockList.Length; i++) {
            if (i == num) {
                rockList[i].SetActive(true);
               
            }
        }




       /* for(int i = 0; i < rockList.Length; i++) {
            if(i == num) {
                dataImage.GetComponent<Image>().sprite = rockList[i];
            }
        }


        switch(num) {
            case 4:
               
                dataImage.GetComponent<Image>().sprite = rockList[0];
            break;

            case 5:
                dataImage.GetComponent<Image>().sprite = rockList[1];
            break;

            default:
           
                dataImage.GetComponent<Image>().sprite = defaultImage;
            break;

        }*/
    }

    public void ResetImage() {
       // dataImage.GetComponent<Image>().sprite = defaultImage;
        for(int i = 0; i < rockList.Length; i++) {
            rockList[i].SetActive(false);
        }
        defaultImage.SetActive(true);
    }
}

