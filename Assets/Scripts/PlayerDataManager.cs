using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the data specific to the player
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    // The player's name
    public string player_name;

    // The player's wallet
    public int money;

    // What day it is
    public int day;


    // The object who's children are the player's storage objects
    public GameObject inventory_obj;

    // The ingredients in the player's inventory, and their positions
    private Ingredient[] inventory = new Ingredient[9];

    // The quantities of the ingredients at each position
    private int[] quantities = new int[9];

    // The shop tiers
    Dictionary<PartList.Part.Tags, int> tier_dict = new Dictionary<PartList.Part.Tags, int>() { { PartList.Part.Tags.plant, 1 }, { PartList.Part.Tags.meat, 1 }, { PartList.Part.Tags.mineral, 1 } };

    private void Start()
    {
        // Set up the inventory with empty ingredients at 0 quantity
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = ScriptableObject.CreateInstance<Ingredient>();
            quantities[i] = 0;
        }

        if (inventory_obj)
        {
            // Assign those empty ingredients to the storage objects
            for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
            {
                inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(inventory[invSlot], invSlot);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (GameObject.FindGameObjectWithTag("ShopManager"))
        {
            ShopManager shopManager = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>();
            shopManager.SetTier(tier_dict[shopManager.shopTag]);
        }
    }

    public void RefreshInventory()
    {
        if (inventory_obj = GameObject.FindGameObjectWithTag("Inventory"))
        {
            //Refresh the storage objects with what's supposed to be there
            for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
            {

                inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(inventory[invSlot], invSlot);
                inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().quantity = quantities[invSlot];

            }
        }
    }

    private void Update()
    {
        if (inventory_obj)
        {
            // Loop through every inventory slot
            for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
            {
                quantities[invSlot] = inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().quantity;

                // If it has run out of an ingredient
                if (quantities[invSlot] <= 0)
                {
                    // Assign it an empty ingredient
                    inventory[invSlot] = ScriptableObject.CreateInstance<Ingredient>();
                    inventory[invSlot].sprite = Resources.Load<Sprite>("Sprites/Empty");
                    inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(inventory[invSlot], invSlot);
                }
            }
        }

    }

    // Return the ID array of the ingredient at a slot
    public int[] GetIDsAtSlot(int slot)
    {
        return inventory[slot].ids;
    }

    public void SetInventoryAtSlot(int slot, Ingredient ingredient, int quantity)
    {
        inventory[slot] = ingredient;
        quantities[slot] = quantity;
    }

    // Add a specific ingredient to a particular slot
    public void AddToInventoryAtSlot(int slot, Ingredient ingredient)
    {
        // If this inventory slot already contains this ingredient
        if (CompareIDs(inventory[slot].ids, ingredient.ids))
        {
            if (inventory_obj)
            {
                // Increment the quantity
                inventory_obj.transform.GetChild(slot).GetComponent<Storage>().quantity++;
            }
        }
        else
        {
            // Assign that ingredient to the inventory slot, to the storage object, and set the quantity to 1
            inventory[slot] = ingredient;
            if (inventory_obj)
            {
                inventory_obj.transform.GetChild(slot).GetComponent<Storage>().AssignIngredient(inventory[slot], slot);
                inventory_obj.transform.GetChild(slot).GetComponent<Storage>().quantity = 1;
            }
        }

        // Update the quantities list
        quantities[slot] = inventory_obj.transform.GetChild(slot).GetComponent<Storage>().quantity;
    }

    // Return the ingredient at a particular slot
    public Ingredient GetIngredientAtSlot(int slot)
    {
        return inventory[slot];
    }

    // Remove an ingredient from a particular slot
    public void RemoveIngredientAtSlot(int slot)
    {
        Ingredient empty = ScriptableObject.CreateInstance<Ingredient>();

        inventory[slot] = empty;
        quantities[slot] = 0;
    }

    // Swap the ingredients at 2 locations, and their quantities
    public void SwapIngredientLocs(int slot1, int slot2, Ingredient ing1, Ingredient ing2)
    {

        int quantity1 = inventory_obj.transform.GetChild(slot1).GetComponent<Storage>().quantity;
        int quantity2 = inventory_obj.transform.GetChild(slot2).GetComponent<Storage>().quantity;

        AddToInventoryAtSlot(slot1, ing2);
        AddToInventoryAtSlot(slot2, ing1);

        inventory_obj.transform.GetChild(slot1).GetComponent<Storage>().quantity = quantity2;
        inventory_obj.transform.GetChild(slot2).GetComponent<Storage>().quantity = quantity1;
        quantities[slot1] = quantity2;
        quantities[slot2] = quantity1;
    }

    // Compare the ids of two ingredients
    public bool CompareIDs(int[] ids1, int[] ids2)
    {

        for (int i = 0; i < ids1.Length; i++)
        {
            if (ids1[i] != ids2[i])
            {
                return false;
            }
        }

        return true;
    }

    // Return the quantity at a particular inventory slot
    public int GetQuantityAtSlot(int slot)
    {
        return quantities[slot];
    }

    // Set the quantity at a particular 
    public void SetQuantityAtSlot(int slot, int quantity)
    {
        quantities[slot] = quantity;
        if (inventory_obj)
        {
            inventory_obj.transform.GetChild(slot).GetComponent<Storage>().quantity = quantity;
        }
    }

    // Set the player's money
    public void SetMoney(int newMoney)
    {
        money = newMoney;
    }

    // Add to the player's money
    public void AddMoney(int increase)
    {
        money += increase;
    }

    // Return the player's money
    public int GetMoney()
    {
        return money;
    }

    // Return true if the player can afford the price
    public bool CanAfford(int price)
    {
        return money >= price;
    }

    // Remove money from the player
    public void Purchase(int price)
    {
        money -= price;
        FindObjectOfType<AudioManager>().Play("Buy/Sell", 1f);
    }

    public void SetTierForTag(PartList.Part.Tags tag, int newTier)
    {
        tier_dict[tag] = newTier;
    }

    public int GetTierFromTag(PartList.Part.Tags tag)
    {
        return tier_dict[tag];
    }

    public void NextDay()
    {
        day++;
        GetComponent<GameManager>().GetGenerator().DumpShopInventories();
    }
}
