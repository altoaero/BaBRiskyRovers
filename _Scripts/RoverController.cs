using UnityEngine;

public class RoverController : MonoBehaviour
{
    public float speed = 5f; // Speed of the rover
    private Sprite leftFacingSprite; // Reference to the sprite for the rover facing left
    private Sprite rightFacingSprite; // Reference to the sprite for the rover facing right

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public AudioClip crashSound; // Add this line
    private AudioSource audioSource; // Add this line

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Add this line
    }

    public void ChangeLeft(Sprite left)
    {
        leftFacingSprite = left;
    }

    public void ChangeRight(Sprite right)
    {
        rightFacingSprite = right;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * speed * Time.deltaTime;

        // Check for horizontal input and change the sprite accordingly
        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // Change to OnCollisionEnter2D for 2D collisions
    {
        if (collision.gameObject.tag == "Rock")
        {
            // Play crash sound
            audioSource.PlayOneShot(crashSound);
            Debug.Log("Rover collided with rock.");
            // Implement health reduction logic here if needed
        }
    }
}
