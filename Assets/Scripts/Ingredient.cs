using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple Scriptable Object class that holds information for randomly generated or manually created ingredients;
/// </summary>
[CreateAssetMenu(fileName = "NewIngredient", menuName = "ScriptableObjects/IngredientScriptableObject", order = 1)]
public class Ingredient : ScriptableObject
{
    public int[] ids = new int[3] { 0, 0, 0};
    public string name = "";
    public int cost = 0;
    public Dictionary<string, float[]> effectsDict = new Dictionary<string, float[]>() { { "str", new float[2] { 0f, 1f } }, { "int", new float[2] { 0f, 1f } }, { "dex", new float[2] { 0f, 1f } } };
    public List<PartList.Part.Tags> tags = new List<PartList.Part.Tags>();
    public string desc_string = "";

    public Sprite sprite = null;
}
