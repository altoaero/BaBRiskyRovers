using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class NewBehaviourScript : MonoBehaviour
{
    public Button transferButton; // Assign the button from the Inspector
    public List<Texture2D> textures = new List<Texture2D>(); // List of Texture2D images
    private int currentTextureIndex = 0; // Index of the current Texture2D being saved

    void Start()
    {
        if (textures.Count == 0)
        {
            Debug.LogError("No Texture2D images assigned.");
            return;
        }

        transferButton.onClick.AddListener(TransferImage);
    }

    void TransferImage()
    {
        if (textures.Count == 0)
        {
            Debug.LogError("No Texture2D images assigned.");
            return;
        }

        Texture2D texture2D = textures[currentTextureIndex];

        if (texture2D == null)
        {
            Debug.LogError("Current Texture2D is not assigned.");
            return;
        }

        if (!texture2D.isReadable)
        {
            Debug.LogError("Texture2D is not readable. Please enable the Read/Write option in the texture import settings.");
            return;
        }

        // Convert Texture2D to PNG
        byte[] bytes = texture2D.EncodeToPNG();

        // Get the Pictures folder path
        string picturesFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
        string fileName = $"SavedImage_{currentTextureIndex}.png";
        string path = Path.Combine(picturesFolderPath, fileName);

        // Write the bytes to a file
        File.WriteAllBytes(path, bytes);

        // Log the path for debugging purposes
        Debug.Log("Image saved to: " + path);

        // Update to the next texture
        currentTextureIndex = (currentTextureIndex + 1) % textures.Count;
    }
}
