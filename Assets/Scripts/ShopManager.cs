using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PartList.Part.Tags shopTag= PartList.Part.Tags.plant;

    public GameObject inventory_obj;

    private Ingredient[] shopInventory = new Ingredient[9];

    private IngredientGenerator generator;


    private void Start()
    {
        generator = GameObject.Find("IngredientGenerator").GetComponent<IngredientGenerator>();

        List<Ingredient> generated = new List<Ingredient>();
        generated = generator.GenerateIngredients(shopInventory.Length, shopTag);



        for (int i = 0; i < shopInventory.Length; i++)
        {
            shopInventory[i] = generated[i];
        }

        for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
        {
            inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(shopInventory[invSlot]);
        }
    }
}
