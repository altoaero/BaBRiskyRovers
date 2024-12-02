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
    private double decay_rate_battery;  
    private double solarpanel_fail_rate;
    private double wheel_fail_rate;
    private double cpu_fail_rate;
    private double circut_fail_rate;
    private double Decay_lambda_solar_panel = 2.5e-05;
    private double Decay_lambda_wheel = 2e-05;
    private double Decay_lambda_cpu = 2.5e-06;
    private double Decay_lambda_circuts = 3e-06;
    private double Decay_lambda_battery = 3e-06;
    private float time_track;
    private double percent_battery;
    private double percent_wheel;
    private double percent_cpu;
    private double percent_circuts;
    private double percent_solar_panel;
    private double percent_circuits;
    private double overall_chasis_rate;
    private double percent_chasis;
    private double one_over_sixty = 1.0 / 60.0;
    private double chasis_exploding;
    public int battery_amount = 3;
    public int cpu_amount = 3;
    public int wheel_amount = 6;
    public int solarp_amount = 3;
    private int wheel_loss_value = 0;
    //the lambdas here can be used for longer sessions failes at 2.3 hours
    private double wheel_lambda_adj = 1 - Math.Pow(1 - 2e-05, 1/60);
    private double cpu_lambda_adj = 1 - Math.Pow(1 - 2.5e-06, 1 / 60);
    private double circut_lambda_adj = 1 - Math.Pow(1 - 3e-06, 1 / 60);
    private double sol_lambda_adj = 1 - Math.Pow(1 - 2.5e-05, 1 / 60);
   /// <summary>
   /// private double batter_lambda_adj = 1 - Math.Pow(1 - 3e-06, 1 / 60);
   /// </summary>
    // 1-(1-lamda)^(1/60)
    void Start()
    {
        double F_Percent = 0.05;
        double power_used = 1.0 / 365;
        double delta_trigger = 365;

      //  double decay_rate_example = 1 - Math.Exp(example * 0);
  

        
        
        //f is difficulity 
        // Calculate decay rate and update the class-level Decay_rate
        // double powerResult = Math.Pow(F_Percent, power_used);
        double ln_1subf = 1 - 0.05;
       // double numerator = (1 - ln_1subf);
        decay_rate_battery = -(Math.Log(ln_1subf) / delta_trigger); // Set the class-level Decay_rate
        //battery_fail_rate = Decay_rate * Math.Exp(-Decay_rate * 0);
        battery_fail_rate =0;
        wheel_fail_rate = 0;
        cpu_fail_rate = 0;
        solarpanel_fail_rate = 0;
        circut_fail_rate = 0;   
    
    }

    // Update is called once per frame
    void Update()
    {
        // Here you could track time or update decay rate if needed
    }

    public int DamageTaken(double health)
    {

        chasis_exploding = (1 - (health / 100)) * (overall_chasis_rate *100);
        double exlpoding = 100 * chasis_exploding;
        Random rnd = new Random();
        int randomNumber = rnd.Next(0, 100);
        if(randomNumber <= exlpoding)
        {
            Debug.Log("chasis exploded at " + exlpoding);
            Debug.Log("heath is " + health);
            return 1;
        }
        else
        {
            Debug.Log("chasis not dead yet at " + exlpoding);
            Debug.Log("heath is " + health);
            return 0;
            
        }


    }

    public void ProbabilitySpinner(float time_passed)
    {
        // Use the class-level Decay_rate for the calculation
        if (wheel_amount >= 2)
        {
            Random wheel_loss_gen = new Random();
            wheel_loss_value = wheel_loss_gen.Next(1, wheel_amount);
        }

        time_track += time_passed;
        //battery_fail_rate += Decay_rate * Math.Exp(-Decay_rate * time_passed);
        battery_fail_rate += 1 - Math.Exp(-Decay_lambda_battery * time_passed);
        wheel_fail_rate += (1 - Math.Exp(-Decay_lambda_wheel * time_passed) * wheel_amount);
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
        if (randomNumber <= percent_battery)
        {
            if (battery_amount < 1)
            {
                Debug.Log("Out of batteries");
                battery_amount = 0;
            }
            else{
                battery_amount -= 1;
                Debug.Log("reaming amount of batteries " + battery_amount);
                battery_fail_rate = 0;
            }
        }
        if (randomNumber <= percent_wheel)
        {
            wheel_amount -= wheel_loss_value;
            if (wheel_amount < 1)
            {
                Debug.Log("Can't move anymore");
                wheel_amount = 0;
            }
            else
            {
                wheel_amount -= 1;
                Debug.Log("reaming amount of wheels " + wheel_amount);
                wheel_fail_rate = 0; 

            }
        }
        if (randomNumber <= percent_cpu)
        {
            if (cpu_amount < 1)
            {
                Debug.Log("Out of cpus");
                cpu_amount = 0;
            }
            else
            {
                cpu_amount -= 1;
                Debug.Log("reaming amount of cpus " + cpu_amount);
                cpu_fail_rate = 0;
            }
        }
        if (randomNumber <= percent_solar_panel)
        {
            if (solarp_amount < 1)
            {
                Debug.Log("can't recharge");
                solarp_amount = 0;
            }
            else
            {
                solarp_amount -= 1;
                Debug.Log("Panel remaining " + solarp_amount);
                solarpanel_fail_rate = 0;
            }
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
           
       // Debug.Log("incremented by " + incremented_by);
    }
}

