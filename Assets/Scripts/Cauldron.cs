using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles the logic for the potion brewing cauldron!
/// </summary>
public class Cauldron : MonoBehaviour, IPointerDownHandler
{
    // The draggable icon prefab
    public GameObject icon_prefab;

    // Variables for holding the various cauldron objects, and the mutable fill colour
    [SerializeField]
    private GameObject fillObj;
    private Color fillColour;
    private Image imageComponent;

    // The list of ingredients that have been thrown into the cauldron
    private List<Ingredient> contents = new List<Ingredient>();

    // Dictionary for holding the effects of the current potion
    public Dictionary<string, float> effects_dict = new Dictionary<string, float>() { { "str", 0f }, {"int", 0f }, {"dex", 0f } };

    // string for making the current potion effects nicer to read
    private string effect_string = "";


    void Start()
    {
        // Get the color of the fill image
        fillColour = fillObj.GetComponent<Image>().color;
    }

    void Update()
    {
        // Only show something in the cauldron if there is actually something in the cauldron
        if(contents.Count > 0)
        {
            fillObj.SetActive(true);
        }
        else
        {
            fillObj.SetActive(false);
        }
    }

    // When the player clicks on the cauldron
    public void OnPointerDown(PointerEventData eventData)
    {
        // If there is a potion currently in the cauldron
        if (contents.Count >= 1)
        {
            // Make a new potion object, and set its data to the cauldron's
            Potion newPotion = ScriptableObject.CreateInstance<Potion>();
            newPotion.effects_dict = effects_dict;
            newPotion.effect_string = effect_string;

            // Create a draggable ui element to hold that potion
            GameObject draggedIcon = Instantiate(icon_prefab, Input.mousePosition, Quaternion.identity, GameObject.Find("Icons").transform);
            UIElementDragger dragger = draggedIcon.GetComponent<UIElementDragger>();
            dragger.dragging = true; dragger.spawn = transform.gameObject; dragger.SetPotion(newPotion);

            // Dump the cauldron
            EmptyCauldron();
        }
    }

    /// <summary>
    /// Add an ingredient to the cauldron
    /// </summary>
    /// <param name="ingredient">The ingredient to add</param>
    public void AddIngredientToCauldron(Ingredient ingredient)
    {
        // If the player is attempting to put a duplicate ingredient in the cauldron, the ingredient is wasted.
        if (!contents.Contains(ingredient))
        {
            // Add the ingredient
            contents.Add(ingredient);

            // Update the effect of the current potion
            CalculateEffects();
            // Update the effect string
            effect_string = BuildEffectString();
            // Update the tooltip
            GetComponent<Display_Tooltip>().SetTooltipText(effect_string);
            // Update the fill color
            fillObj.GetComponent<Image>().color = BuildFillColour();
        }
    }

    /// <summary>
    /// Calculate the current effect of the potion based on the cauldron's contents
    /// </summary>
    private void CalculateEffects()
    {
        effects_dict["str"] = 0f;
        effects_dict["int"] = 0f;
        effects_dict["dex"] = 0f;

        // Add all of the stat modifiers together
        foreach (Ingredient ingredient in contents)
        {
            Debug.Log(ingredient.effectsDict["str"][0]);
            effects_dict["str"] += ingredient.effectsDict["str"][0];
            Debug.Log(ingredient.effectsDict["int"][0]);
            effects_dict["int"] += ingredient.effectsDict["int"][0];
            Debug.Log(ingredient.effectsDict["dex"][0]);
            effects_dict["dex"] += ingredient.effectsDict["dex"][0];
        }
        // Apply the stat multipliers to the collated modifiers
        foreach (Ingredient ingredient in contents)
        {
            effects_dict["str"] *= ingredient.effectsDict["str"][1];
            effects_dict["int"] *= ingredient.effectsDict["int"][1];
            effects_dict["dex"] *= ingredient.effectsDict["dex"][1];
        }

    }

    /// <summary>
    /// Dump the contents of the cauldron
    /// </summary>
    private void EmptyCauldron()
    {
        contents.Clear();
        fillObj.GetComponent<Image>().color = Color.white;
        effect_string = "";
        GetComponent<Display_Tooltip>().SetTooltipText(effect_string);
    }


    /// <summary>
    /// Generate the new fill
    /// </summary>
    /// <returns></returns>
    private Color BuildFillColour()
    {
        Color newColour = fillColour;

        // Lerp between the old color and the new ingredient's color
        foreach(Ingredient ingredient in contents)
        {
            newColour = Color.Lerp(newColour, ingredient.sprite.texture.GetPixel(64, 64), 0.5f);
        }

        return newColour;
    }

    /// <summary>
    /// Build the effect string
    /// </summary>
    /// <returns>The new effect string</returns>
    private string BuildEffectString()
    {
        // Create empty strings to hold the text
        string finalString = "";

        string str_string = "";
        string int_string = "";
        string dex_string = "";
        
        // Skip any stats that don't have modifiers
        if (effects_dict["str"] != 0f)
        {
            // Colour the string to make it nice
            str_string = "<color=red>STR: ";
           
            // Decide whether or not to use a "+" sign, or just to leave it blank if there is a negative modifier
            str_string += ((effects_dict["str"] < 0) ? "" : "+") + effects_dict["str"];
            
            str_string += "</color> ";
        }
        if (effects_dict["int"] != 0f)
        {
            int_string = "<color=blue>INT: ";

            int_string += ((effects_dict["int"] < 0) ? "" : "+") + effects_dict["int"];

            int_string += "</color> ";
        }
        if (effects_dict["dex"] != 0f)
        {
            dex_string = "<color=green>DEX: ";

            dex_string += ((effects_dict["dex"] < 0) ? "" : "+") + effects_dict["dex"];

            dex_string += "</color>";
        }

        // Put it all together
        finalString = str_string + int_string + dex_string;

        return finalString;
    }
}
