using UnityEngine;

public class RoverController : MonoBehaviour
{
    public float speed = 5f; // Speed of the rover
    public Sprite leftFacingSprite;  // Reference to the sprite for the rover facing left
    public Sprite rightFacingSprite; // Reference to the sprite for the rover facing right

    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite to facing right
        spriteRenderer.sprite = rightFacingSprite;
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
            spriteRenderer.sprite = leftFacingSprite;
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.sprite = rightFacingSprite;
        }
    }
}
