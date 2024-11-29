using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBarSlider; // Reference to the Health Bar Slider
    public GameObject gameOverPanel; // Reference to the Game Over Panel
    public Button launchSecondRoverButton; // Reference to the Launch Second Rover Button
    public GameObject gravePrefab; // Reference to the Grave Prefab
    public Transform roverTransform; // Reference to the Rover's Transform
    public DecayingParts decayingParts; // Reference to DecayingParts
    private bool isGameOver = false;
    private int chasis;

    void Awake()
    {
        // Initialize health values
        currentHealth = maxHealth;  // Ensure currentHealth is set to maxHealth at the start
       // Debug.Log("Current health value " + currentHealth);
        healthBarSlider.maxValue = maxHealth;
       // Debug.Log("Health bar slider max value " + healthBarSlider.maxValue);
        healthBarSlider.value = currentHealth;
        //Debug.Log("Health bar slider value " + healthBarSlider.value);
        //debug later but the code is referenced twice 
        // prefab MainHealthBar
        // prefab Rover
        // Additional setup before Start()
    }

    void Start()
    {


        decayingParts = FindObjectOfType<DecayingParts>();
        // Hide the Game Over Panel at the start
        gameOverPanel.SetActive(false);

        // Hide the launch rover button at the start
        launchSecondRoverButton.onClick.AddListener(OnLaunchSecondRover);
        launchSecondRoverButton.gameObject.SetActive(false);
    }

    void Update()
    {
        // Only update health bar if the game is not over
      /*  if (!isGameOver)
        {
            UpdateHealthBar();
        } */

        // Debug log to track health during the game loop
       
    }

    void TakeDamage(int damage)
    {
        if (!isGameOver)
        {
            currentHealth -= damage;
            chasis = decayingParts.DamageTaken(currentHealth);
            if (chasis == 1)
            {
                currentHealth = 0;
            }
            // Prevent health from going below zero
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            // Log after taking damage to ensure currentHealth is changing
          
            UpdateHealthBar();
            //Send to decay script 
            // If health reaches 0, trigger Game Over
            if (currentHealth == 0)
            {
                ShowGameOver();
            }
        }
    }

    void UpdateHealthBar()
    {
        // Ensure slider value is updated correctly and debug the slider value
        healthBarSlider.value = currentHealth;
        
    }

    void ShowGameOver()
    {
        // When game is over, show the Game Over panel
        isGameOver = true;
        gameOverPanel.SetActive(true);
        launchSecondRoverButton.gameObject.SetActive(true);
    }

    void OnLaunchSecondRover()
    {
        // Reset health and show the grave
        currentHealth = maxHealth;
        isGameOver = false;
        UpdateHealthBar();
        gameOverPanel.SetActive(false);
        launchSecondRoverButton.gameObject.SetActive(false);

        Debug.Log("OnLaunchSecondRover() - Health reset to Max: " + currentHealth);  // Debugging reset
    }

    public void ReceiveTimeUpdate()
    {
        TakeDamage(1);  // Decrease health by 5 every 5 seconds (or whatever the interval is)
    }
}

