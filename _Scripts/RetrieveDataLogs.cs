using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveDataLogs : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 checkpointPosition;
    private Vector3 previousVelocity;
    private Vector3 orginposition;    
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
    List<string> movementstatus1 = new List<string>();
    private string movementsPrinted = "";
    private float x_value; 
    private float y_value;
    public float t_value; //time value 
    public int h_value; // health value 
    public HealthManager healthManager;
    public GameTimer interaltime;
    private List<(float time, float x, float y, int health)> movementData = new List<(float, float, float, int)>();

    void Start()
    {

        previousPosition = rover.transform.position;
        orginposition = rover.transform.position;
        Debug.Log(previousPosition);
        checkpointPosition = rover.transform.position;
        previousVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        if (healthManager == null)
        {
            healthManager = FindObjectOfType<HealthManager>();
        }
        if (interaltime == null)
        {
            interaltime = FindObjectOfType<GameTimer>();
        }
      
    }

    void Update()
    {
        Vector3 currentPosition = rover.transform.position;
        
        x_value = currentPosition.x;
        y_value = currentPosition.y;
        t_value = interaltime.GetElapsedTime();
        h_value = healthManager.currentHealth;

       
        var movementstatus_current = (time: t_value,x: x_value, y: y_value, health: h_value);
        //movementsPrinted = movementstatus_current.ToString();
        movementData.Add(movementstatus_current);

       
        // Vector2 displacement = currentPosition - previousPosition;

        // Calculate velocity as displacement per second
        // currentVelocity = displacement / Time.deltaTime;

        // Find the direction of the velocities
        /*     if (previousVelocity.x == 0.0) {
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
                     direction = "North";
                 }
                 else if (cvx == 0 && cvy == -1) {
                     direction = "South";
                 }
                 else if (cvx == 1 && cvy == 0) {
                     direction = "East";
                 }
                 else if (cvx == -1 && cvy == 0) {
                     direction = "West";
                 }
                 else if (cvx == 1 && cvy == 1) {
                     direction = "Northeast";
                 }
                 else if (cvx == -1 && cvy == 1) {
                     direction = "Northwest";
                 }
                 else if (cvx == -1 && cvy == -1) {
                     direction = "Southwest";
                 }
                 else if (cvx == 1 && cvy == -1) {
                     direction = "Southeast";
                 }
                 else {
                     direction = "Back to Base";
                 }

             } """*/

        // Debugging
        //Debug.Log("Velocity: " + currentVelocity);

        // Update previousVelocity to currentVelocity for the next frame
        //previousPosition = currentPosition;
        //previousVelocity = currentVelocity;
    }

     public string printMovements() {
        movementsPrinted = "";
        foreach (var data in movementData)
        {
            // Format each tuple entry as a string
            string formattedEntry = $"Time: {data.time:F2}s | Position: ({data.x:F2}, {data.y:F2}) | Health: {data.health}";
            movementsPrinted += formattedEntry + "\n";
            Debug.Log(formattedEntry);
        }

        return movementsPrinted;
    }
   
}
