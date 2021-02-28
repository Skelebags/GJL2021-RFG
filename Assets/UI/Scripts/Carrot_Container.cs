using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot_Container : Storage
{
    // Start is called before the first frame update
    void Start()
    {
        // Grab the game manager
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        Ingredient carrot = ScriptableObject.CreateInstance<Ingredient>();

        carrot.ingredient_name = "Carrot";
        carrot.ids = new int[3] { 999, 999, 999 };
        carrot.cost = 0;
        carrot.effectsDict = new Dictionary<string, float[]>();

        carrot.effectsDict.Add("str", new float[2] { 0, 1 });
        carrot.effectsDict.Add("int", new float[2] { 0, 1 });
        carrot.effectsDict.Add("dex", new float[2] { 3, 1 });

        carrot.desc_string = "A simple carrot.";
        carrot.effect_string = IngredientGenerator.BuildEffectString(carrot.effectsDict);
        carrot.tags.Add(PartList.Part.Tags.plant);

        carrot.sprite = Resources.Load<Sprite>("Sprites/Carrot_Graphic");

        quantity = 5 + GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>().day;

        AssignIngredient(carrot, 10);
    }

}
