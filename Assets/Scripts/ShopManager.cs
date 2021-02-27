using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles shop logic
/// </summary>
public class ShopManager : MonoBehaviour
{
    // This shop's tag, used for generating ingredients
    public PartList.Part.Tags shopTag= PartList.Part.Tags.plant;

    // The object who's children are the shop's storage objects
    public GameObject inventory_obj;

    // The shops inventory of ingredients
    private Ingredient[] shopInventory = new Ingredient[9];

    // The scene's ingredient generator
    private IngredientGenerator generator;

    [SerializeField]
    private int tier = 1;


    private void Start()
    {
        // Grab the ingredient Generator
        generator = GameObject.FindGameObjectWithTag("IngredientGenerator").GetComponent<IngredientGenerator>();

        // Generate a group of appropriate unique ingredients
        List<Ingredient> generated = new List<Ingredient>();
        generated = generator.GenerateIngredients(shopInventory.Length, shopTag, tier);

        // Assign those ingredients to the shop's inventory
        for (int i = 0; i < shopInventory.Length; i++)
        {
            shopInventory[i] = generated[i];
        }

        // Assign the ingredients to their respective storage
        for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
        {
            inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(shopInventory[invSlot], invSlot);
        }
    }

    public void SetTier(int newTier)
    {
        tier = newTier;
    }

    public int GetTier()
    {
        return tier;
    }
}
