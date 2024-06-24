using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The rover to follow
    public float smoothSpeed = 0.125f; // Smooth speed for camera movement
    public Vector3 offset; // Offset for the camera

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
