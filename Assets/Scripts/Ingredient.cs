using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "ScriptableObjects/IngredientScriptableObject", order = 2)]
public class Ingredient : ScriptableObject
{
    public ComponentList.Component[] components = new ComponentList.Component[3];

    public string name;
    public int cost;
    public List<string> effects = new List<string>();
}
