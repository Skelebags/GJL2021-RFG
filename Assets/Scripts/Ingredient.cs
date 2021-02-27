using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple Scriptable Object class that holds information for randomly generated or manually created ingredients;
/// </summary>
[CreateAssetMenu(fileName = "NewIngredient", menuName = "ScriptableObjects/IngredientScriptableObject", order = 0)]
public class Ingredient : ScriptableObject
{
    // The IDs that correspond to this ingredient's parts
    public int[] ids = new int[3] { 0, 0, 0};

    public string ingredient_name = "";
    public int cost = 0;

    // Dictionary of the ingredient's effects
    public Dictionary<string, float[]> effectsDict;

    // List of tags
    public List<PartList.Part.Tags> tags = new List<PartList.Part.Tags>();

    public string desc_string = "";
    public string effect_string = "";

    public Sprite sprite = null;

}
