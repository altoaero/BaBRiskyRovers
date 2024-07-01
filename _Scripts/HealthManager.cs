using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100; // Total max health for the combined health bar
    private float currentHealth;
    public Slider bigHealthBarSlider; // Reference to the big Health Bar Slider

    public MovHealthManager movementHealthManager; // Reference to the script managing movement health
    public RockHealthManager collisionHealthManager; // Reference to the script managing collision health

    private float movementHealth;
    private float collisionHealth;

    void Start()
    {
        currentHealth = maxHealth;
        bigHealthBarSlider.maxValue = maxHealth;
        bigHealthBarSlider.value = currentHealth;

        // Initialize individual health managers
        movementHealthManager.onHealthChanged += UpdateMovementHealth;
        collisionHealthManager.onHealthChanged += UpdateCollisionHealth;
    }

    void UpdateMovementHealth(float newMovementHealth)
    {
        movementHealth = newMovementHealth;
        UpdateCombinedHealth();
    }

    void UpdateCollisionHealth(float newCollisionHealth)
    {
        collisionHealth = newCollisionHealth;
        UpdateCombinedHealth();
    }

    void UpdateCombinedHealth()
    {
        // Calculate the combined health
        currentHealth = movementHealth + collisionHealth;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            LoadGameOverScene(); // Load the game over scene
        }
    }

    void UpdateHealthBar()
    {
        bigHealthBarSlider.value = currentHealth;
    }

    void LoadGameOverScene()
    {
        SceneManager.LoadScene("Died"); // Replace "GameOverScene" with the name of your scene
    }
}
