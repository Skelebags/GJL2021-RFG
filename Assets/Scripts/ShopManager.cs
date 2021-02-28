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
    private int[] quantities = new int[9];

    // The scene's ingredient generator
    private IngredientGenerator generator;

    [SerializeField]
    private int tier;

    private List<Ingredient> generated = new List<Ingredient>();


    private void Start()
    {
        // Get our shop tier
        tier = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>().GetTierFromTag(shopTag);

        // Grab the ingredient Generator
        generator = GameObject.FindGameObjectWithTag("IngredientGenerator").GetComponent<IngredientGenerator>();

        // Generate a group of appropriate unique ingredients
        
        generated = generator.GenerateIngredients(shopInventory.Length, shopTag, tier);
        quantities = generator.GetQuantities(shopTag);

        // Assign those ingredients to the shop's inventory
        for (int i = 0; i < shopInventory.Length; i++)
        {
            shopInventory[i] = generated[i];
        }

        // Assign the ingredients to their respective storage
        for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
        {
            inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(shopInventory[invSlot], invSlot);
            inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().quantity = quantities[invSlot];
        }
    }

    private void Update()
    {
        for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
        {
            quantities[invSlot] = inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().quantity;
        }

        generator.UpdateShopInventory(shopTag, shopInventory, quantities);
    }

    public void SetTier(int newTier)
    {
        tier = newTier;
    }

    public int GetTier()
    {
        return tier;
    }

    public void Upgrade()
    {
        tier++;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>().SetTierForTag(shopTag, tier);
    }
}
