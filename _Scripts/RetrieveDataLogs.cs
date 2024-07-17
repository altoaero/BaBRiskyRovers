using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveDataLogs : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 checkpointPosition;
    private Vector3 previousVelocity;
    public GameObject rover;
    private Vector3 currentVelocity;
    private float distanceMoved;
    private bool zero;
    private int pvx;
    private int pvy;
    private int cvx;
    private int cvy;
    private string direction;
    List<string> movements = new List<string>();
    private string movementsPrinted = "";

    void Start()
    {
        previousPosition = rover.transform.position;
        checkpointPosition = rover.transform.position;
        previousVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        Vector3 currentPosition = rover.transform.position;
        Vector2 displacement = currentPosition - previousPosition;
        
        // Calculate velocity as displacement per second
        currentVelocity = displacement / Time.deltaTime;

        // Find the direction of the velocities
        if (previousVelocity.x == 0.0) {
            pvx = 0;
        }
        else if (previousVelocity.x < 0.0) {
            pvx = -1;
        }
        else if (previousVelocity.x > 0.0) {
            pvx = 1;
        }

        if (previousVelocity.y == 0.0) {
            pvy = 0;
        }
        else if (previousVelocity.y < 0.0) {
            pvy = -1;
        }
        else if (previousVelocity.y > 0.0) {
            pvy = 1;
        }

        if (currentVelocity.x == 0.0) {
            cvx = 0;
        }
        else if (currentVelocity.x < 0.0) {
            cvx = -1;
        }
        else if (currentVelocity.x > 0.0) {
            cvx = 1;
        }

        if (currentVelocity.y == 0.0) {
            cvy = 0;
        }
        else if (currentVelocity.y < 0.0) {
            cvy = -1;
        }
        else if (currentVelocity.y > 0.0) {
            cvy = 1;
        }

        if (cvx != pvx || cvy != pvy) {
            
            // Calculate the distance moved since the start
            distanceMoved = Vector3.Distance(checkpointPosition, currentPosition);

            // Add the distance moved to the "movements" array
            if (distanceMoved > 0.1) {
                movements.Add(direction + " " + distanceMoved.ToString($"F{4}") + " m");
            }

            // Reset previousPosition;
            checkpointPosition = currentPosition;

            // Identify the direction
            if (cvx == 0 && cvy == 1) {
                direction = "Forward";
            }
            else if (cvx == 0 && cvy == -1) {
                direction = "Backward";
            }
            else if (cvx == 1 && cvy == 0) {
                direction = "Right";
            }
            else if (cvx == -1 && cvy == 0) {
                direction = "Left";
            }
            else if (cvx == 1 && cvy == 1) {
                direction = "Forward and Right";
            }
            else if (cvx == -1 && cvy == 1) {
                direction = "Forward and Left";
            }
            else if (cvx == -1 && cvy == -1) {
                direction = "Backward and Left";
            }
            else if (cvx == 1 && cvy == -1) {
                direction = "Backward and Right";
            }
            else {
                direction = "";
            }

        }

        // Debugging
        //Debug.Log("Velocity: " + currentVelocity);
        
        // Update previousVelocity to currentVelocity for the next frame
        previousPosition = currentPosition;
        previousVelocity = currentVelocity;
    }

    public string printMovements() {
        foreach (string item in movements) {
            movementsPrinted += item + "\n";
        }
        return movementsPrinted;
    }
}
