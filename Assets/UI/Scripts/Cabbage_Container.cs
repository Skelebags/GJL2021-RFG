using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabbage_Container : Storage
{
    // Start is called before the first frame update
    void Start()
    {
        // Grab the game manager
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        Ingredient cabbage = ScriptableObject.CreateInstance<Ingredient>();

        cabbage.ingredient_name = "Cabbage";
        cabbage.ids = new int[3] { 999, 999, 999 };
        cabbage.cost = 0;
        cabbage.effectsDict = new Dictionary<string, float[]>();

        cabbage.effectsDict.Add("str", new float[2] { 0, 1 });
        cabbage.effectsDict.Add("int", new float[2] { 3, 1 });
        cabbage.effectsDict.Add("dex", new float[2] { 0, 1 });

        cabbage.desc_string = "A simple cabbage.";
        cabbage.effect_string = IngredientGenerator.BuildEffectString(cabbage.effectsDict);
        cabbage.tags.Add(PartList.Part.Tags.plant);

        cabbage.sprite = Resources.Load<Sprite>("Sprites/Cabbage_Graphic");

        quantity = 5 + GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>().day;

        AssignIngredient(cabbage, 10);
    }

}
