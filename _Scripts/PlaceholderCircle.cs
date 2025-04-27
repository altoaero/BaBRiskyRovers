using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderCircle : MonoBehaviour
{
    public int textureSize = 128;
    public Color circleColor = Color.white;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        CreateCircle();
    }

    void CreateCircle()
    {
        // Create a new texture
        Texture2D texture = new Texture2D(textureSize, textureSize);

        // Draw a circle on the texture
        float radius = textureSize / 2f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                texture.SetPixel(x, y, distance <= radius ? circleColor : Color.clear);
            }
        }

        texture.Apply();

        // sprite from the texture
        Sprite circleSprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f));

        // Assign the sprite to a SpriteRenderer
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = circleSprite;
    }

    // Call this method to replace the circle with the actual asset
    public void ReplaceWithAsset(Sprite newSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }
}
