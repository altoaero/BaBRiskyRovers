using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Transactions;
using Random = System.Random;

public class DecayingParts : MonoBehaviour
{
    private double battery_fail_rate;
    private double Decay_rate_battery;  // Class-level variable
    private double solarpanel_fail_rate;
    private double wheel_fail_rate;
    private double cpu_fail_rate;
    private double circut_fail_rate;
    private double Decay_lambda_solar_panel = 2.5e-02;
    private double Decay_lambda_wheel = 2.5e-02;
    private double Decay_lambda_cpu = 6.1e-03;
    private double Decay_lambda_circuts = 7e-03;
    private float time_track;
    private double percent_battery;
    private double percent_wheel;
    private double percent_cpu;
    private double percent_circuts;
    private double percent_solar_panel;
    private double percent_circuits;
    private double overall_chasis_rate;
    private double percent_chasis;
    void Start()
    {
        double F_Percent = 0.05;
        double power_used = 1.0 / 365;
        double delta_trigger = 365;
        //f is difficulity 
        // Calculate decay rate and update the class-level Decay_rate
       // double powerResult = Math.Pow(F_Percent, power_used);
        double ln_1subf = 1 - 0.05;
       // double numerator = (1 - ln_1subf);
        Decay_rate_battery = -(Math.Log(ln_1subf) / delta_trigger); // Set the class-level Decay_rate
        //battery_fail_rate = Decay_rate * Math.Exp(-Decay_rate * 0);
        battery_fail_rate = 1 - Math.Exp(-Decay_rate_battery * 0);
        wheel_fail_rate = 1 - Math.Exp(-Decay_lambda_wheel * 0);
        cpu_fail_rate = 1-Math.Exp(-Decay_lambda_cpu * 0);
        solarpanel_fail_rate = 1 - Math.Exp(-solarpanel_fail_rate * 0);
     
        circut_fail_rate = 1 - Math.Exp(-Decay_lambda_circuts * 0);
       
        // Log decay rate to verify
        Debug.Log("Decay rate is " + Decay_rate_battery);
        Debug.Log("Rate of failure at 0 seconds " + battery_fail_rate);
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
        //battery_fail_rate += Decay_rate * Math.Exp(-Decay_rate * time_passed);
        battery_fail_rate += 1 - Math.Exp(-Decay_rate_battery * time_passed);
        wheel_fail_rate += 1 - Math.Exp(-Decay_lambda_wheel * time_passed);
        cpu_fail_rate += 1 - Math.Exp(-Decay_lambda_cpu * time_passed);
        solarpanel_fail_rate += 1 - Math.Exp(-Decay_lambda_solar_panel * time_passed);
        circut_fail_rate += 1 - Math.Exp(-Decay_lambda_circuts * time_passed);
        
        percent_wheel = 100 * wheel_fail_rate;
        percent_battery = 100 * battery_fail_rate;
        percent_cpu = 100 * cpu_fail_rate;
        percent_solar_panel= 100 * solarpanel_fail_rate;
        percent_circuits = 100 * circut_fail_rate;

        overall_chasis_rate = battery_fail_rate * wheel_fail_rate * cpu_fail_rate * solarpanel_fail_rate * circut_fail_rate;
        //       double incremented_by = 1 - Math.Exp(-Decay_rate_battery* time_passed);
        percent_chasis = 100 * overall_chasis_rate;
        Debug.Log("overall fail percentage " + overall_chasis_rate);
        Random rnd = new Random();
        int randomNumber = rnd.Next(0, 100);
        if (randomNumber >= percent_battery)
        {
            Debug.Log("Battery lives at " + percent_battery+ "%" + "  " + randomNumber);
        }
        else
        {
            Debug.Log("Battery dies at " + percent_battery + "%" + "  "+randomNumber);
        }
        if (randomNumber >= percent_wheel)
        {
            Debug.Log("Wheel lives at " + percent_wheel + "%" + "  " + randomNumber);
        }
        else
        {
            Debug.Log("Wheel dies at " + percent_wheel + "%" + "  " + randomNumber);
        }
        if (randomNumber >= percent_cpu)
        {
            Debug.Log("CPU lives at " + percent_cpu + "%" + "  " + randomNumber);
        }
        else
        {
            Debug.Log("CPU dies at " + percent_cpu + "%" + "  " + randomNumber);
        }

        if (randomNumber >= percent_solar_panel)
        {
            Debug.Log("Solar Panel lives at " + percent_solar_panel + "%" + "  " + randomNumber);
        }
        else
        {
            Debug.Log("Solar Panel dies at " + percent_solar_panel + "%" + "  " + randomNumber);
        }

        if (randomNumber >= percent_circuits)
        {
            Debug.Log("circuits lives at " + percent_circuits + "%" + "  " + randomNumber);
        }
        else
        {
            Debug.Log("Circuits dies at " + percent_circuits + "%" + "  " + randomNumber);
        }
        if (randomNumber >= percent_chasis)
        {
            Debug.Log("Chasis lives at " + percent_chasis + "%" + "  " + randomNumber);
        }
        else
        {
            Debug.Log("Chasis dies at " + percent_chasis + "%" + "  " + randomNumber);
        }






            // Log the failure rate
            Debug.Log("Fail rate of battery is " + battery_fail_rate+" at "+ time_track);
       // Debug.Log("incremented by " + incremented_by);
    }
}

