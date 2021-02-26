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
        if (partList == null)
        {
            partList = GetComponent<PartList>();
            partList.PopulatePartsFromJSON();

            SplitData();
        }
    }

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
    public List<Ingredient> GenerateIngredients(int numberOfIngredients, PartList.Part.Tags targetTag)
    {

        List<Ingredient> Ingredients = new List<Ingredient>();


        for (int i = 0; i < numberOfIngredients; i++)
        {
            int loopCounter = 0;
            int MAX_LOOPS = 50;

            Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();
            do
            {
                PartList.Part colourPart = null;
                PartList.Part descPart = null;
                PartList.Part typePart = null;

                do
                {
                    int randIndex = Random.Range(0, Colours.Count);
                    if (Colours[randIndex].tags.Contains(PartList.Part.Tags.any) || Colours[randIndex].tags.Contains(targetTag))
                    {
                        colourPart = Colours[randIndex];
                    }
                } while (colourPart == null);

                do
                {
                    int randIndex = Random.Range(0, Descriptors.Count);
                    if (Descriptors[randIndex].tags.Contains(PartList.Part.Tags.any) || Descriptors[randIndex].tags.Contains(targetTag))
                    {
                        descPart = Descriptors[randIndex];
                    }
                } while (descPart == null);

                do
                {
                    int randIndex = Random.Range(0, Types.Count);
                    if (Types[randIndex].tags.Contains(PartList.Part.Tags.any) || Types[randIndex].tags.Contains(targetTag))
                    {
                        typePart = Types[randIndex];
                    }
                } while (typePart == null);

                ingredient.ids[0] = colourPart.id;
                ingredient.ids[1] = descPart.id;
                ingredient.ids[2] = typePart.id;

                ingredient.name = colourPart.name + " " + descPart.name + " " + typePart.name;
                ingredient.cost = colourPart.cost + descPart.cost + typePart.cost;


                ingredient.effectsDict["str"] = new float[2] { (colourPart.effectsDict["str"][0] + descPart.effectsDict["str"][0] + typePart.effectsDict["str"][0]), (colourPart.effectsDict["str"][1] * descPart.effectsDict["str"][1] * typePart.effectsDict["str"][1]) };
                ingredient.effectsDict["int"] = new float[2] { (colourPart.effectsDict["int"][0] + descPart.effectsDict["int"][0] + typePart.effectsDict["int"][0]), (colourPart.effectsDict["int"][1] * descPart.effectsDict["int"][1] * typePart.effectsDict["int"][1]) };
                ingredient.effectsDict["dex"] = new float[2] { (colourPart.effectsDict["dex"][0] + descPart.effectsDict["dex"][0] + typePart.effectsDict["dex"][0]), (colourPart.effectsDict["dex"][1] * descPart.effectsDict["dex"][1] * typePart.effectsDict["dex"][1]) };

                ingredient.tags.AddRange(colourPart.tags);
                ingredient.tags.AddRange(descPart.tags);
                ingredient.tags.AddRange(typePart.tags);

                ingredient.sprite = CombineSprites(colourPart.sprite, descPart.sprite, typePart.sprite);

                ingredient.desc_string = colourPart.desc_string + typePart.desc_string + descPart.desc_string;

                ingredient.effect_string = BuildEffectString(ingredient.effectsDict);

                loopCounter++;

            } while (Ingredients.Contains(ingredient) && loopCounter < MAX_LOOPS);

            Ingredients.Add(ingredient);
        }
        
        return Ingredients;
    }

    public Ingredient GenerateIngredientFromIDs(int colourId, int descId, int typeId)
    {
        Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();

        PartList.Part colourPart = partList.Data[colourId];
        PartList.Part descPart = partList.Data[descId];
        PartList.Part typePart = partList.Data[typeId];

        ingredient.ids[0] = colourPart.id;
        ingredient.ids[1] = descPart.id;
        ingredient.ids[2] = typePart.id;

        ingredient.name = colourPart.name + " " + descPart.name + " " + typePart.name;
        ingredient.cost = colourPart.cost + descPart.cost + typePart.cost;

        ingredient.effectsDict = new Dictionary<string, float[]>();

        ingredient.effectsDict.Add("str", new float[2] { (colourPart.effectsDict["str"][0] + descPart.effectsDict["str"][0] + typePart.effectsDict["str"][0]), (colourPart.effectsDict["str"][1] * descPart.effectsDict["str"][1] * typePart.effectsDict["str"][1]) });
        ingredient.effectsDict.Add("int", new float[2] { (colourPart.effectsDict["int"][0] + descPart.effectsDict["int"][0] + typePart.effectsDict["int"][0]), (colourPart.effectsDict["int"][1] * descPart.effectsDict["int"][1] * typePart.effectsDict["int"][1]) });
        ingredient.effectsDict.Add("dex", new float[2] { (colourPart.effectsDict["dex"][0] + descPart.effectsDict["dex"][0] + typePart.effectsDict["dex"][0]), (colourPart.effectsDict["dex"][1] * descPart.effectsDict["dex"][1] * typePart.effectsDict["dex"][1]) });
        //ingredient.effectsDict["int"] = new float[2] { (colourPart.effectsDict["int"][0] + descPart.effectsDict["int"][0] + typePart.effectsDict["int"][0]), (colourPart.effectsDict["int"][1] * descPart.effectsDict["int"][1] * typePart.effectsDict["int"][1]) };
        //ingredient.effectsDict["dex"] = new float[2] { (colourPart.effectsDict["dex"][0] + descPart.effectsDict["dex"][0] + typePart.effectsDict["dex"][0]), (colourPart.effectsDict["dex"][1] * descPart.effectsDict["dex"][1] * typePart.effectsDict["dex"][1]) };

        ingredient.tags.AddRange(colourPart.tags);
        ingredient.tags.AddRange(descPart.tags);
        ingredient.tags.AddRange(typePart.tags);

        ingredient.sprite = CombineSprites(colourPart.sprite, descPart.sprite, typePart.sprite);

        ingredient.desc_string = colourPart.desc_string + typePart.desc_string + descPart.desc_string;
        ingredient.effect_string = BuildEffectString(ingredient.effectsDict);

        return ingredient;
    }

    private Sprite CombineSprites(Sprite colourSprite, Sprite descSprite, Sprite typeSprite)
    {
        Sprite finalSprite;

        Texture2D tex = new Texture2D(128, 128);


        tex = Instantiate(typeSprite.texture);

        for(int y = 0; y < tex.height; y++)
        {
            for(int x = 0; x < tex.width; x++)
            {
                if(tex.GetPixel(x, y) == Color.white)
                {
                    Color newColour = colourSprite.texture.GetPixel(x, y) * descSprite.texture.GetPixel(x, y);
                    tex.SetPixel(x, y, newColour);
                }
            }
        }

        tex.Apply();


        finalSprite = Sprite.Create(tex, new Rect(0, 0, 128, 128), Vector2.zero);

        return finalSprite;
    }

    private string BuildEffectString(Dictionary<string, float[]> effectsDict)
    {
        string finalString = "";

        string str_string = "";
        string int_string = "";
        string dex_string = "";

        if (!(effectsDict["str"][0] == 0 && effectsDict["str"][1] == 1))
        {
            str_string = "<color=red>STR: ";
            if (effectsDict["str"][0] != 0)
            {
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
        
        finalString = str_string + int_string + dex_string;

        return finalString;
    }
}
