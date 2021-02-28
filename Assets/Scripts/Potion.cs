using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A small class to hold the potion data
/// </summary>
[CreateAssetMenu(fileName = "NewPotion", menuName = "ScriptableObjects/PotionScriptableObject", order = 1)]
public class Potion : ScriptableObject
{
    // The potion's effect dictionary
    public Dictionary<string, float> effects_dict = new Dictionary<string, float>() { { "str", 0f }, { "int", 0f }, { "dex", 0f } };

    // The potion's sprite
    public Sprite sprite;

    // The readable effect string
    public string effect_string = "";

    public Sprite BuildSprite(Color newColour, Sprite baseSprite)
    {
        // Create a placeholder sprite
        Sprite finalSprite;

        // Create a new texture to edit
        Texture2D tex = new Texture2D(128, 128);

        // Assign that texture to an instance of the Type Part's texture (we do not want to edit the original)
        tex = Instantiate(baseSprite.texture);

        // Loop through every pixel in the texture
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                // If any given pixel is white
                if (tex.GetPixel(x, y) == Color.white)
                {
                    tex.SetPixel(x, y, newColour);
                }
            }
        }

        // Apply the texture changes
        tex.Apply();

        // Setup the new sprite
        finalSprite = Sprite.Create(tex, new Rect(0, 0, 128, 128), Vector2.zero);

        // Return the sprite
        return finalSprite;
    }
}


