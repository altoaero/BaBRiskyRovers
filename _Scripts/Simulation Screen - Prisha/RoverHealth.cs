using UnityEngine;
using UnityEngine.UI;

public class RoverHealth : MonoBehaviour
{
    public Slider healthBar;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        // Initialize the health
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the rover collided with a rock
        if (collision.gameObject.CompareTag("Rock"))
        {
            TakeDamage(10f);
        }
    }

    void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Update the health bar slider
        healthBar.value = currentHealth;
    }
}
