using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGenerator : MonoBehaviour
{
    public const int INGREDIENT_COUNT = 1;

    public List<Ingredient> Ingredients = new List<Ingredient>();

    private ComponentList componentList;

    // Start is called before the first frame update
    void Start()
    {
        componentList = GetComponent<ComponentList>();

        componentList.PopulateComponentsFromJSON();

        GenerateIngredients();
        Debug.Log(Ingredients.Count);

        Debug.Log(Ingredients[0].name);
        Debug.Log(Ingredients[0].cost);
        foreach(string effect in Ingredients[0].effects)
        {
            Debug.Log(effect);
        }
    }

    private void GenerateIngredients()
    {
        for(int i = 0; i < INGREDIENT_COUNT; i++)
        {
            Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();
            ingredient.name = componentList.Data[0].name + " " + componentList.Data[1].name + " " + componentList.Data[2].name;
            ingredient.cost = 10;
            ingredient.effects.AddRange(componentList.Data[0].effects);
            ingredient.effects.AddRange(componentList.Data[1].effects);
            ingredient.effects.AddRange(componentList.Data[2].effects);
            Ingredients.Add(ingredient);
        }
    }
}
