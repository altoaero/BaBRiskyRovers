using UnityEngine;

public class Rock10ClickHandler : MonoBehaviour
{
    public GameObject popupPanel; // Reference to the popup panel
    private bool isPopupOpen = false;

    void Start()
    {
        ClosePopup();
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
}
