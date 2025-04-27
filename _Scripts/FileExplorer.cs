using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FileExplorer : MonoBehaviour
{
    string path; // hold the path of the selected file
    public GameObject rover; // hold a reference to the rover GameObject

    public void OpenExplorer()
    {
        // rover testing
        rover = GameObject.Find("Rover");
        if (rover == null)
        {
            Debug.LogError("? Rover GameObject not found in the scene!");
            return;
        }
        Debug.Log($"? Rover found: {rover.name}");

        //  fileselecting
#if UNITY_EDITOR
        Debug.Log("??? Opening file explorer (Editor-only)...");
        path = EditorUtility.OpenFilePanel("Select Movement File", "", "txt");
#else
    Debug.LogWarning("?? File Explorer only works in Unity Editor");
    return;
#endif

        // file path validation
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("?? No file selected!");
            return;
        }
        Debug.Log($"?? Selected file: {path}");

        // taking the file path
        string fileName = Path.GetFileName(path);
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("? Failed to extract filename!");
            return;
        }
        Debug.Log($"?? Processing file: {fileName}");

        // moving by float values slowly.
        try
        {
            switch (fileName.ToLower())
            {
                case "forward-10.txt":
                    Debug.Log("?? Moving FORWARD (+X) by 0.1 unit");
                    rover.transform.Translate(0.1f, 0, 0);
                    break;
                case "backward-10.txt":
                    Debug.Log("?? Moving BACKWARD (-X) by  0.1 unit");
                    rover.transform.Translate(-0.1f, 0, 0);
                    break;
                case "downward-10.txt":
                    Debug.Log("?? Moving DOWNWARD (-Y) by  0.1 unit");
                    rover.transform.Translate(0, -0.1f, 0);
                    break;
                case "upward-10.txt":
                    Debug.Log("?? Moving UPWARD (+Y) by  0.1 unit");
                    rover.transform.Translate(0, 0.1f, 0);
                    break;
                default:
                    Debug.LogError($"? Unknown movement file: {fileName}");
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"?? Movement error: {e.Message}");
        }
    }
}
