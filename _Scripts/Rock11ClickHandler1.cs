using UnityEngine;

public class Rock11ClickHandler : MonoBehaviour
{
    public GameObject popupPanel; // Reference to the popup panel
    public Transform roverTransform; // Reference to the rover's transform
    public float proximityDistance = 1f; // Distance within which the popup will stay open

    private bool isPopupOpen = false;

    void Start()
    {
        ClosePopup();
    }

    void Update()
    {
        if (isPopupOpen && !IsRoverInProximity())
        {
            ClosePopup();
        }
    }

    void OnMouseDown()
    {
        if (isPopupOpen)
        {
            ClosePopup();
        }
        else
        {
            OpenPopup();
        }
    }

    void OpenPopup()
    {
        popupPanel.SetActive(true);
        isPopupOpen = true;
        // You can add additional logic here based on the specific rock prefab
    }

    void ClosePopup()
    {
        popupPanel.SetActive(false);
        isPopupOpen = false;
    }

    bool IsRoverInProximity()
    {
        return Vector3.Distance(transform.position, roverTransform.position) <= proximityDistance;
    }
}
