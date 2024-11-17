using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timeElapsed; // Time elapsed since the game started
    public float timeLimit = 1800f; // 30 minutes = 1800 seconds

    private float timeSinceLastLog = 0f; // To track the time since the last log
    public float logInterval = 5f; // Log every 5 seconds

    public HealthManager healthManager; // Reference to HealthManager
    public DecayingParts decayingParts; // Reference to DecayingParts
    private bool timerActive = true; // Flag to track if the timer is active

    void Start()
    {
        // Initialize timer to 0 at the start
        timeElapsed = 0f;

        // Find the HealthManager and DecayingParts in the scene
        healthManager = FindObjectOfType<HealthManager>();
        decayingParts = FindObjectOfType<DecayingParts>();

        // Log an error if healthManager or decayingParts is missing
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found in the scene! Please make sure it is present.");
        }
        if (decayingParts == null)
        {
            Debug.LogError("DecayingParts not found in the scene! Please make sure it is present.");
        }
    }

    void Update()
    {
        // If the timer is not active, skip updating
        if (!timerActive)
            return;

        // Increase the timer by the time passed since the last frame
        timeElapsed += Time.deltaTime;

        // Track time since the last log
        timeSinceLastLog += Time.deltaTime;

        // Log every `logInterval` seconds (in this case, every 5 seconds)
        if (timeSinceLastLog >= logInterval)
        {
            // Check if healthManager and decayingParts are not null before calling methods
            if (healthManager != null)
            {
                healthManager.ReceiveTimeUpdate(); // Call method in HealthManager to update health
            }

            if (decayingParts != null)
            {
                decayingParts.ProbabilitySpinner(timeSinceLastLog); // Update decaying parts
            }

           timeSinceLastLog = 0f;  // Reset the timer for the next interval
        }

        // If the timer exceeds the time limit, trigger some event (e.g., Game Over, level complete)
        if (timeElapsed >= timeLimit)
        {
            OnTimeUp();
        }

        // If health reaches zero, stop the timer
        if (healthManager != null && healthManager.currentHealth <= 0)
        {
            StopTimer(); // Stop the timer if health reaches zero
        }
    }

    // Method to stop the timer
    private void StopTimer()
    {
        timerActive = false;
        Debug.Log("Health reached zero! Timer stopped.");
    }

    // A method that gets triggered when the timer reaches the limit
    private void OnTimeUp()
    {
        // Implement what happens when the timer hits the time limit (e.g., trigger game over or end the level)
        Debug.Log("Time's up!");

        // Add logic for game over or restart, if needed
        // Example: GameManager.Instance.GameOver();
    }

    // Optionally, you can get the elapsed time in seconds (for other uses)
    public float GetElapsedTime()
    {
        return timeElapsed;
    }
}
