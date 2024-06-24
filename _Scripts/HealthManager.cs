using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public Slider healthBarSlider; // Reference to the Health Bar Slider
    public Image fillImage;

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

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
    }
}
