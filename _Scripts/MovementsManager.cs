using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MovementsManager : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public Button myButton; // Reference to the Button
    public RetrieveDataLogs retrieveDataLogs;

    
    // Start is called before the first frame update
    void Start()
    {
        retrieveDataLogs = GameObject.Find("RetrieveDataLogs").GetComponent<RetrieveDataLogs>();
        mainText.text = "No Movement Yet";
        // Register the button click event
        myButton.onClick.AddListener(updateDataLog);
    }

    void updateDataLog() {
        mainText.text = retrieveDataLogs.printMovements();
    }

}
