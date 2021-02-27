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
}
