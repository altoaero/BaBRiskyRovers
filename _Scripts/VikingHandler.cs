using UnityEngine;

public class VikingHandler : MonoBehaviour
{
    public GameObject popup; // Assign the popup Panel in the Inspector
    public GameObject rover; // Assign the rover GameObject in the Inspector
    public float proximityDistance = 5f; // Define the distance for proximity check

    private bool isPopupActive = false;

    void Start()
    {
        // Ensure the popup is hidden when the game starts
        popup.SetActive(false);
    }

    void Update()
    {
        // Check the distance between the game object and the rover
        float distance = Vector3.Distance(transform.position, rover.transform.position);

        // If the popup is active and the rover moves out of proximity, close the popup
        if (isPopupActive && distance > proximityDistance)
        {
            popup.SetActive(false);
            isPopupActive = false;
        }
    }

    void OnMouseDown()
    {
        // Toggle the popup visibility when the object is clicked
        isPopupActive = !isPopupActive;
        popup.SetActive(isPopupActive);
    }
}
