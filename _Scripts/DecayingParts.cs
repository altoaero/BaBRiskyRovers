using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DecayingParts : MonoBehaviour
{
    // Start is called before the first frame update
  

    void Start()
    {
        double F_Percent = 0.05;
        double power_used = 1.0 / 360.0;
        double delta_trigger = 5.0;
        double powerResult = Math.Pow(F_Percent, power_used);
        double numerator = (1 - powerResult);
        double Decay_rate = -(Math.Log(numerator)/ delta_trigger);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
