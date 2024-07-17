using UnityEngine;

public class Teleporter : MonoBehaviour
{
  public float targetX = 0.2149899f;
    public float targetY = 0.2917824f;
    public float targetZ = -0.00843029f;

    // Function called when you want to teleport
    public void TeleportToCoordinates()
    {
        // Set the object's position to the target coordinates
        transform.position = new Vector3(targetX, targetY, targetZ);
    }
}