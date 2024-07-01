using UnityEngine;
using UnityEngine.UI;

public class MovHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    public Slider healthBarSlider; // Reference to the Health Bar Slider

    public delegate void HealthChanged(float movementHealth);
    public event HealthChanged onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
    }

    void Update()
    {
        CheckMovement();
    }

    void CheckMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isMoving = true;
        }

        if (isMoving)
        {
            TakeDamage(0.001f); // Reduce health by 0.5 unit
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBar();

        onHealthChanged?.Invoke(currentHealth); // Trigger health changed event
    }

    void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
    }
}
