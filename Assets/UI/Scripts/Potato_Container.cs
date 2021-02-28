using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato_Container : Storage
{
    // Start is called before the first frame update
    void Start()
    {
        // Grab the game manager
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        Ingredient potato = ScriptableObject.CreateInstance<Ingredient>();

        potato.ingredient_name = "Potato";
        potato.ids = new int[3] { 999, 999, 999 };
        potato.cost = 0;
        potato.effectsDict = new Dictionary<string, float[]>();

        potato.effectsDict.Add("str", new float[2] { 3, 1 });
        potato.effectsDict.Add("int", new float[2] { 0, 1 });
        potato.effectsDict.Add("dex", new float[2] { 0, 1 });

        potato.desc_string = "A simple potato.";
        potato.effect_string = IngredientGenerator.BuildEffectString(potato.effectsDict);
        potato.tags.Add(PartList.Part.Tags.plant);

        potato.sprite = Resources.Load<Sprite>("Sprites/Potato_Graphic");

        quantity = 5 + GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>().day;

        AssignIngredient(potato, 10);
    }

}
