using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timeElapsed; // Time elapsed since the game started
    public float timeLimit = 1800f; // 30 minutes = 1800 seconds

    private float timeSinceLastLog = 0f; // To track the time since the last log
    public float logInterval = 5f; // Log every 5 seconds

    void Start()
    {
        timeElapsed = 0f; // Initialize timer to 0 at the start
    }

    void Update()
    {
        // Increase the timer by the time passed since the last frame
        timeElapsed += Time.deltaTime;

        // Track time since the last log
        timeSinceLastLog += Time.deltaTime;

        // Log every `logInterval` seconds (in this case, every 5 seconds)
        if (timeSinceLastLog >= logInterval)
        {
            // Log the time elapsed
            Debug.Log("Time Elapsed: " + timeElapsed.ToString("F2") + " seconds");

            // Reset the log timer
            timeSinceLastLog = 0f;
        }

        // If the timer exceeds the time limit, trigger some event (e.g., Game Over, level complete)
        if (timeElapsed >= timeLimit)
        {
            OnTimeUp();
        }
    }

    // A method that gets triggered when the timer reaches the limit
    private void OnTimeUp()
    {
        // Implement what happens when the timer hits the time limit (e.g., trigger game over or end the level)
        Debug.Log("Time's up!");

        // You can add any other logic here (e.g., restart the level, trigger a UI pop-up, etc.)
        // For example, call another function or change the game state:
        // GameManager.Instance.GameOver();
    }

    // Optionally, you can get the elapsed time in seconds (for other uses)
    public float GetElapsedTime()
    {
        return timeElapsed;
    }
}

