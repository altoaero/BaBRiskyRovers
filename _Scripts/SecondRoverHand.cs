using UnityEngine;
using UnityEngine.UI;

public class SecondRoverHand : MonoBehaviour
{
    public Button myButton; // Reference to the button that will disappear
    public GameObject panel1; // Reference to the first panel that will disappear
    public GameObject panel2; // Reference to the second panel that will disappear

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Deactivate the button and panels
        myButton.gameObject.SetActive(false);
        panel1.SetActive(false);
        panel2.SetActive(false);
    }
}
