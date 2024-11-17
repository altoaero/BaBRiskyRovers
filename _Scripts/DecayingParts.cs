using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DecayingParts : MonoBehaviour
{
    private double battery_fail_rate;
    private double Decay_rate;  // Class-level variable
    private float time_track;
    void Start()
    {
        double F_Percent = 0.05;
        double power_used = 1.0 / 360.0;
        double delta_trigger = 5.0;

        // Calculate decay rate and update the class-level Decay_rate
        double powerResult = Math.Pow(F_Percent, power_used);
        double numerator = (1 - powerResult);
        Decay_rate = -(Math.Log(numerator) / delta_trigger); // Set the class-level Decay_rate

        // Log decay rate to verify
        Debug.Log("Decay rate is " + Decay_rate);
    }

    // Update is called once per frame
    void Update()
    {
        // Here you could track time or update decay rate if needed
    }

    public void ProbabilitySpinner(float time_passed)
    {
        // Use the class-level Decay_rate for the calculation
        time_track += time_passed;
        battery_fail_rate += Decay_rate * Math.Exp(-Decay_rate * time_passed);

        // Log the failure rate
        Debug.Log("Fail rate of battery is " + battery_fail_rate+" at "+ time_track);
    }
}

