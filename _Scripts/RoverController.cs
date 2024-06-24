using UnityEngine;

public class RoverController : MonoBehaviour
{
    public float speed = 5f; // Speed of the rover

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * speed * Time.deltaTime;
    }
}
