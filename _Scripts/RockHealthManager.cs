using UnityEngine;
using UnityEngine.UI;

public class RockHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    public Slider healthBarSlider; // Reference to the Health Bar Slider

    public delegate void HealthChanged(float collisionHealth);
    public event HealthChanged onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            TakeDamage(1);
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
