using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : ScriptableObject
{
    public Dictionary<string, float> effects_dict = new Dictionary<string, float>() { { "str", 0f }, { "int", 0f }, { "dex", 0f } };

    public Sprite sprite;

    public string effect_string = "";
}
