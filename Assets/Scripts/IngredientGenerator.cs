using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Takes the component list data, splits it based on component type, and then randomly generates a number of ingredients
/// </summary>
public class IngredientGenerator : MonoBehaviour
{
    private PartList partList;

    private List<PartList.Part> Colours = new List<PartList.Part>();
    private List<PartList.Part> Descriptors = new List<PartList.Part>();
    private List<PartList.Part> Types = new List<PartList.Part>();

    // Start is called before the first frame update
    void Awake()
    {
        // If we haven't parsed the JSON file, parse it
        if (partList == null)
        {
            partList = GetComponent<PartList>();
            partList.PopulatePartsFromJSON();

            SplitData();
        }
    }

    /// <summary>
    /// Splits the part list into 3 seperate lists based on what slot they occupy
    /// </summary>
    private void SplitData()
    {
        foreach (PartList.Part part in partList.Data.Values)
        {
            switch (part.slot)
            {
                case "Colour":
                    Colours.Add(part);
                    break;
                case "Descriptor":
                    Descriptors.Add(part);
                    break;
                case "Type":
                    Types.Add(part);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Generates an ingredient of a specified tag type
    /// </summary>
    public List<Ingredient> GenerateIngredients(int numberOfIngredients, PartList.Part.Tags targetTag, int shopTier)
    {
        // Create the list we will return
        List<Ingredient> Ingredients = new List<Ingredient>();

        
        for (int i = 0; i < numberOfIngredients; i++)
        {
            int loopCounter = 0;
            int MAX_LOOPS = 50;
            
            // Create the new ingredient
            Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();

            // Keep generating the ingredient ----
            do
            {
                PartList.Part colourPart = null;
                PartList.Part descPart = null;
                PartList.Part typePart = null;

                // Get a random Colour of the targetted tag, or with the "any" tag at a particular tier or below
                do
                {
                    int randIndex = Random.Range(0, Colours.Count);
                    if ((Colours[randIndex].tags.Contains(PartList.Part.Tags.any) || Colours[randIndex].tags.Contains(targetTag)) && Colours[randIndex].tier <= shopTier)
                    {
                        colourPart = Colours[randIndex];
                    }
                } while (colourPart == null);

                // Get a random Descriptor, or with the "any" tag at a particular tier or below
                do
                {
                    int randIndex = Random.Range(0, Descriptors.Count);
                    if ((Descriptors[randIndex].tags.Contains(PartList.Part.Tags.any) || Descriptors[randIndex].tags.Contains(targetTag)) && Descriptors[randIndex].tier <= shopTier)
                    {
                        descPart = Descriptors[randIndex];
                    }
                } while (descPart == null);

                // Get a random Type, or with the "any" tag at a particular tier or below
                do
                {
                    int randIndex = Random.Range(0, Types.Count);
                    if ((Types[randIndex].tags.Contains(PartList.Part.Tags.any) || Types[randIndex].tags.Contains(targetTag)) && Types[randIndex].tier <= shopTier)
                    {
                        typePart = Types[randIndex];
                    }
                } while (typePart == null);

                // Build that ingredient
                ingredient = BuildIngredient(colourPart, descPart, typePart);

                // Keep track of how many times we've tried this, do not infinite loop
                loopCounter++;

            } while (Ingredients.Contains(ingredient) && loopCounter < MAX_LOOPS); // --- Until the ingredient is unique, or the we've tried the maximum number of times

            // Add the ingredient to the list
            Ingredients.Add(ingredient);
        }
        
        // Return the list
        return Ingredients;
    }
    

    /// <summary>
    /// Create a specific ingredient from 3 specific part IDs
    /// </summary>
    /// <param name="colourId">The ID of the colour part</param>
    /// <param name="descId">The ID of the descriptor part</param>
    /// <param name="typeId">The ID of the</param>
    /// <returns>An ingredient built from the supplied parts</returns>
    public Ingredient GenerateIngredientFromIDs(int colourId, int descId, int typeId)
    {
        Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();

        // Grab the parts from the JSON data
        PartList.Part colourPart = partList.Data[colourId];
        PartList.Part descPart = partList.Data[descId];
        PartList.Part typePart = partList.Data[typeId];

        // Build the ingredient
        ingredient = BuildIngredient(colourPart, descPart, typePart);

        // Return the ingredient
        return ingredient;
    }

    /// <summary>
    /// Builds the ingredient data from 3 giving parts
    /// </summary>
    /// <param name="colourPart">The desired colour part</param>
    /// <param name="descPart">The desired descriptor part</param>
    /// <param name="typePart">The desired type part</param>
    /// <returns></returns>
    private Ingredient BuildIngredient(PartList.Part colourPart, PartList.Part descPart, PartList.Part typePart)
    {
        Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();

        // Populate the ingredient's ID array with its part ids
        ingredient.ids[0] = colourPart.id;
        ingredient.ids[1] = descPart.id;
        ingredient.ids[2] = typePart.id;

        // Build the ingredient's name from the part names
        ingredient.ingredient_name = colourPart.part_name + " " + descPart.part_name + " " + typePart.part_name;

        // The ingredient's cost is the sum of the parts'
        ingredient.cost = colourPart.cost + descPart.cost + typePart.cost;

        ingredient.effectsDict = new Dictionary<string, float[]>();
       
        // The ingredient's effects are all the part modifiers added together, and all the multipliers added together
        // Multipliers are kept separate at this point. They are applied when the potion is brewed
        ingredient.effectsDict.Add("str", new float[2] { (colourPart.effectsDict["str"][0] + descPart.effectsDict["str"][0] + typePart.effectsDict["str"][0]), (colourPart.effectsDict["str"][1] * descPart.effectsDict["str"][1] * typePart.effectsDict["str"][1]) });
        ingredient.effectsDict.Add("int", new float[2] { (colourPart.effectsDict["int"][0] + descPart.effectsDict["int"][0] + typePart.effectsDict["int"][0]), (colourPart.effectsDict["int"][1] * descPart.effectsDict["int"][1] * typePart.effectsDict["int"][1]) });
        ingredient.effectsDict.Add("dex", new float[2] { (colourPart.effectsDict["dex"][0] + descPart.effectsDict["dex"][0] + typePart.effectsDict["dex"][0]), (colourPart.effectsDict["dex"][1] * descPart.effectsDict["dex"][1] * typePart.effectsDict["dex"][1]) });

        // Add the part tags to the ingredient
        ingredient.tags.AddRange(colourPart.tags);
        ingredient.tags.AddRange(descPart.tags);
        ingredient.tags.AddRange(typePart.tags);

        // Build the ingredients new sprite
        Debug.Log(typePart.part_name);
        ingredient.sprite = CombineSprites(colourPart.sprite, descPart.sprite, typePart.sprite);

        // Combine the part flavour text
        ingredient.desc_string = colourPart.desc_string + typePart.desc_string + descPart.desc_string;

        // Build the effect string
        ingredient.effect_string = BuildEffectString(ingredient.effectsDict);

        // Return the ingredient
        return ingredient;
    }

    /// <summary>
    /// Combines the sprites from 3 given parts into an ingredient sprite
    /// </summary>
    /// <param name="colourSprite">The sprite from the colour part</param>
    /// <param name="descSprite">The sprite from the descriptor part</param>
    /// <param name="typeSprite">The sprite from the type part</param>
    /// <returns></returns>
    private Sprite CombineSprites(Sprite colourSprite, Sprite descSprite, Sprite typeSprite)
    {
        // Create a placeholder sprite
        Sprite finalSprite;

        // Create a new texture to edit
        Texture2D tex = new Texture2D(128, 128);

        // Assign that texture to an instance of the Type Part's texture (we do not want to edit the original)
        Debug.Log(typeSprite.texture.name);
        tex = Instantiate(typeSprite.texture);

        // Loop through every pixel in the texture
        for(int y = 0; y < tex.height; y++)
        {
            for(int x = 0; x < tex.width; x++)
            {
                // If any given pixel is white
                if(tex.GetPixel(x, y) == Color.white)
                {
                    // Replace that pixel with the combined colour of the colour and descriptor at that pixel coordinate
                    Color newColour = colourSprite.texture.GetPixel(x, y) * descSprite.texture.GetPixel(x, y);
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

    /// <summary>
    /// Builds the effect string from the effects dictionary in a way that is nice and readable
    /// </summary>
    /// <param name="effectsDict">The ingredient's effect dictionary</param>
    /// <returns>The pretty effects string</returns>
    private string BuildEffectString(Dictionary<string, float[]> effectsDict)
    {
        // Create placeholders
        string finalString = "";

        string str_string = "";
        string int_string = "";
        string dex_string = "";

        // Only actually add sections that have an effect (no +0s or x1s)
        if (!(effectsDict["str"][0] == 0 && effectsDict["str"][1] == 1))
        {
            // Colour the text to make it nice
            str_string = "<color=red>STR: ";
            if (effectsDict["str"][0] != 0)
            {
                // Do we want a "+" for positive numbers?
                str_string += ((effectsDict["str"][0] < 0) ? "" : "+") + effectsDict["str"][0];
            }
            if (effectsDict["str"][1] != 1)
            {
                str_string += " x" + effectsDict["str"][1];
            }
            str_string += "</color>";
        }
        if (!(effectsDict["int"][0] == 0 && effectsDict["int"][1] == 1))
        {
            // If we already have text, add a line break
            if(str_string != "")
            {
                int_string += "\n";
            }
            int_string += "<color=blue>INT: ";
            if (effectsDict["int"][0] != 0)
            {
                int_string += ((effectsDict["int"][0] < 0) ? "" : "+") + effectsDict["int"][0];
            }
            if (effectsDict["int"][1] != 1)
            {
                int_string += " x" + effectsDict["int"][1];
            }
            int_string += "</color>";
        }
        if (!(effectsDict["dex"][0] == 0 && effectsDict["dex"][1] == 1))
        {
            if (str_string != "" || int_string != "")
            {
                dex_string += "\n";
            }
            dex_string += "<color=green>DEX: ";
            if (effectsDict["dex"][0] != 0)
            {
                dex_string += ((effectsDict["dex"][0] < 0) ? "" : "+") + effectsDict["dex"][0];
            }
            if (effectsDict["dex"][1] != 1)
            {
                dex_string += " x" + effectsDict["dex"][1];
            }
            dex_string += "</color>";
        }
        
        // Put it all together!
        finalString = str_string + int_string + dex_string;

        return finalString;
    }
}
