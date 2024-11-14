using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The rover to follow
    public float smoothSpeed = 0.125f; // Smooth speed for camera movement
    public Vector3 offset; // Offset for the camera
    public Camera camera; // Camera to change settings for

    void LateUpdate()
    {
        // Set the orthographic size to the FOV value set by the player
        float cameraSize = PlayerPrefs.GetFloat("fovValue");
        if (cameraSize > 0) { // Check the cameraSize is a plausible value
            camera.orthographicSize = cameraSize;
        }
        else { 
            camera.orthographicSize = 0.5f;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}