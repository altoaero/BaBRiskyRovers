using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System;
public class FileExplorerComposition : MonoBehaviour
{
    public Button openPicturesButton; // hold a reference to the Button component

    void Start()
    {
        // Register the OpenPicturesFolder method to be called when the button is clicked
        openPicturesButton.onClick.AddListener(OpenPicturesFolder);
    }

    void OpenPicturesFolder()
    {
        // Open the Pictures folder using Windows Explorer
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
    }
}
