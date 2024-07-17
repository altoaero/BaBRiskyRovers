using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class FileExplorer : MonoBehaviour
{
    string path;
    public GameObject rover;


    public void OpenExplorer()
    {
        rover = GameObject.Find("Rover");

        string basePath = "C:\\Commands\\movement-files";
        path = EditorUtility.OpenFilePanel("","Commands" , "movement-files");
        if (path.Length != 0)
       {
           int startIndex = basePath.Length+1; // Index of the comma
            string modifiedString = path.Substring(startIndex);
          
            if (modifiedString == "forward-10.txt")
            {
                rover.transform.Translate(1f, 0,0);
            }

            else if (modifiedString == "backward-10.txt")
            {
                rover.transform.Translate(-1f, 0,0);
            }

            else if (modifiedString == "downward-10.txt")
            {
                rover.transform.Translate(0, -1f,0);
            }
              else if (modifiedString == "upward-10.txt")
            {
                rover.transform.Translate(0, 1f,0);
            }
        }
        else
        {
            Debug.Log("no name");

        }
    }
}
