using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public Slider healthBarSlider; // Reference to the Health Bar Slider
    public GameObject gameOverPanel; // Reference to the Game Over Panel
    public Button launchSecondRoverButton; // Reference to the Launch Second Rover Button
    public GameObject gravePrefab; // Reference to the Grave Prefab
    public Transform roverTransform; // Reference to the Rover's Transform

    private bool isGameOver = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
        gameOverPanel.SetActive(false); // Make sure the Game Over Panel is hidden at the start

        // Add listener to the button
        launchSecondRoverButton.onClick.AddListener(OnLaunchSecondRover);
        launchSecondRoverButton.gameObject.SetActive(false); // Ensure the button is initially hidden
    }

    void Update()
    {
        if (!isGameOver)
        {
            UpdateHealthBar();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGameOver && collision.gameObject.CompareTag("Rock"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        if (!isGameOver)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            UpdateHealthBar();

            if (currentHealth == 0)
            {
                ShowGameOver();
            }
        }
    }

    void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
    }

    void ShowGameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true); // Show the Game Over Panel
        launchSecondRoverButton.gameObject.SetActive(true); // Show the Launch Second Rover Button
    }

    void OnLaunchSecondRover()
    {
        // Calculate the offset position for the grave
        Vector3 gravePosition = roverTransform.position + new Vector3(1.0f, 0, 0); // Adjust the offset as needed

        // Instantiate the grave sprite at the calculated position
        Instantiate(gravePrefab, gravePosition, Quaternion.identity);

        currentHealth = maxHealth;
        isGameOver = false;
        UpdateHealthBar();
        gameOverPanel.SetActive(false); // Hide the Game Over Panel
        launchSecondRoverButton.gameObject.SetActive(false); // Hide the Launch Second Rover Button

        // Permanently disable the button after it is clicked once
        launchSecondRoverButton.onClick.RemoveListener(OnLaunchSecondRover);
        launchSecondRoverButton.gameObject.SetActive(false);
    }
}
