using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Takes the component list data, splits it based on component type, and then randomly generates a number of ingredients
/// </summary>
public class IngredientGenerator : MonoBehaviour
{
    public List<Ingredient> Ingredients = new List<Ingredient>();

    private const int INGREDIENT_COUNT = 5;

    private ComponentList componentList;

    private List<ComponentList.Component> Colours = new List<ComponentList.Component>();
    private List<ComponentList.Component> Descriptors = new List<ComponentList.Component>();
    private List<ComponentList.Component> Types = new List<ComponentList.Component>();

    // Start is called before the first frame update
    void Start()
    {
        componentList = GetComponent<ComponentList>();

        componentList.PopulateComponentsFromJSON();

        SplitData();
        

        GenerateIngredients();

        //Debug.Log(Ingredients.Count);
        //foreach (Ingredient ingredient in Ingredients)
        //{
        //    Debug.Log(ingredient.name);
        //    Debug.Log(ingredient.cost);
        //    foreach (KeyValuePair<string, float[]> effect in ingredient.effectsDict)
        //    {
        //        Debug.Log(effect.Key + ": Modifier => " + effect.Value[0] + " Multiplier => " + effect.Value[1]);
        //    }
        //}

        
    }

    private void SplitData()
    {
        foreach (ComponentList.Component component in componentList.Data)
        {
            switch (component.slot)
            {
                case "Colour":
                    Colours.Add(component);
                    break;
                case "Descriptor":
                    Descriptors.Add(component);
                    break;
                case "Type":
                    Types.Add(component);
                    break;
                default:
                    break;
            }
        }
    }

    private void GenerateIngredients()
    {
        for(int i = 0; i < INGREDIENT_COUNT; i++)
        {
            ComponentList.Component colourComponent = Colours[Random.Range(0, Colours.Count)];
            ComponentList.Component descComponent = Descriptors[Random.Range(0, Descriptors.Count)];
            ComponentList.Component typeComponent = Types[Random.Range(0, Types.Count)];

            Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();
            //ingredient.name = componentList.Data[0].name + " " + componentList.Data[1].name + " " + componentList.Data[2].name;
            //ingredient.cost = 10;
            //ingredient.effects.AddRange(componentList.Data[0].effects);
            //ingredient.effects.AddRange(componentList.Data[1].effects);
            //ingredient.effects.AddRange(componentList.Data[2].effects);

            ingredient.name = colourComponent.name + " " + descComponent.name + " " + typeComponent.name;
            ingredient.cost = colourComponent.cost + descComponent.cost + typeComponent.cost;
            //ingredient.effects.AddRange(colourComponent.effects);
            //ingredient.effects.AddRange(descComponent.effects);
            //ingredient.effects.AddRange(typeComponent.effects);
            //foreach(string effect in colourComponent.effects)
            //{
            //    ingredient.effects += (effect + " ");
            //}
            //foreach (string effect in descComponent.effects)
            //{
            //    ingredient.effects += (effect + " ");
            //}
            //foreach (string effect in typeComponent.effects)
            //{
            //    ingredient.effects += (effect + " ");
            //}

            ingredient.effectsDict["str"] = new float[2] { (colourComponent.effectsDict["str"][0] + descComponent.effectsDict["str"][0] + typeComponent.effectsDict["str"][0]), (colourComponent.effectsDict["str"][1] * descComponent.effectsDict["str"][1] * typeComponent.effectsDict["str"][1]) };
            ingredient.effectsDict["int"] = new float[2] { (colourComponent.effectsDict["int"][0] + descComponent.effectsDict["int"][0] + typeComponent.effectsDict["int"][0]), (colourComponent.effectsDict["int"][1] * descComponent.effectsDict["int"][1] * typeComponent.effectsDict["int"][1]) };
            ingredient.effectsDict["dex"] = new float[2] { (colourComponent.effectsDict["dex"][0] + descComponent.effectsDict["dex"][0] + typeComponent.effectsDict["dex"][0]), (colourComponent.effectsDict["dex"][1] * descComponent.effectsDict["dex"][1] * typeComponent.effectsDict["dex"][1]) };

            Ingredients.Add(ingredient);
        }
    }
}
