using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FileExplorer : MonoBehaviour
{
    string path; // hold the path of the selected file
    public GameObject rover; // hold a reference to the rover GameObject

    public void OpenExplorer()
    {
        // Find the rover GameObject in the scene
        rover = GameObject.Find("Rover");

        // Set the base path for the movement files
        string basePath = "C:\\Commands\\movement-files";

#if UNITY_EDITOR
        // Open a file panel to select a movement file (only in the editor)
        path = EditorUtility.OpenFilePanel("", "Commands", "movement-files");
#endif

        // Check if a file was selected
        if (!string.IsNullOrEmpty(path))
        {
            // Calculate the start index for extracting the file name
            int startIndex = basePath.Length + 1;

            // Extract the file name from the full path
            string modifiedString = path.Substring(startIndex);

            // Move the rover based on the selected file
            if (modifiedString == "East-10.txt")
            {
                rover.transform.Translate(1f, 0, 0); // Move rover East
            }
            else if (modifiedString == "West-10.txt")
            {
                rover.transform.Translate(-1f, 0, 0); // Move rover West
            }
            else if (modifiedString == "South-10.txt")
            {
                rover.transform.Translate(0, -1f, 0); // Move rover South
            }
            else if (modifiedString == "North-10.txt")
            {
                rover.transform.Translate(0, 1f, 0); // Move rover North
            }
        }
        else
        {
            Debug.Log("No file selected"); // Log a message if no file was selected
        }
    }
}
