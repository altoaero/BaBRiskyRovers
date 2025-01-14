using UnityEngine;
using UnityEngine.UI;

public class MultiImageSwitcher : MonoBehaviour
{
    // Array to hold references to the Image components
    public Image[] uiImages;

    // Sprites to toggle between
    public Sprite sprite1;
    public Sprite sprite2;

    // Flag to track which sprite is currently applied
    private bool isUsingSprite1 = true;

    // Method to set all images to the current sprite
    public void SwitchAllImages()
    {
        if (uiImages == null || uiImages.Length == 0 || sprite1 == null || sprite2 == null)
        {
            Debug.LogError("Make sure all required fields are set!");
            return;
        }

        // Choose the sprite to apply
        Sprite currentSprite = isUsingSprite1 ? sprite2 : sprite1;

        // Apply the chosen sprite to all images
        foreach (Image img in uiImages)
        {
            if (img != null)
            {
                img.sprite = currentSprite;
            }
        }

        // Toggle the flag
        isUsingSprite1 = !isUsingSprite1;
    }

    public void SwitchImageAtIndex(int index)
    {
        if (uiImages == null || index < 0 || index >= uiImages.Length)
        {
            Debug.LogError("Invalid index or image array is not set!");
            return;
        }

        // Get the specific image
        Image img = uiImages[index];

        if (img == null)
        {
            Debug.LogError($"Image at index {index} is null!");
            return;
        }

        // Toggle the sprite for the specific image
        img.sprite = img.sprite == sprite1 ? sprite2 : sprite1;
    }
}
