using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public string player_name;

    public int money;

    public GameObject inventory_obj;

    private Ingredient[] inventory = new Ingredient[9];

    private void Start()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = ScriptableObject.CreateInstance<Ingredient>();
        }

        for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
        {
            inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(inventory[invSlot], invSlot);
        }
    }

    private void Update()
    {
        for (int invSlot = 0; invSlot < inventory_obj.transform.childCount; invSlot++)
        {
            if(inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().quantity <= 0)
            {
                inventory[invSlot] = ScriptableObject.CreateInstance<Ingredient>();
                inventory_obj.transform.GetChild(invSlot).GetComponent<Storage>().AssignIngredient(inventory[invSlot], invSlot);
            }
        }
    }

    public int[] GetIDsAtSlot(int slot)
    {
        return inventory[slot].ids;
    }

    public void AddToInventoryAtSlot(int slot, Ingredient ingredient)
    {
        if(CompareIDs(inventory[slot].ids, ingredient.ids))
        {
            inventory_obj.transform.GetChild(slot).GetComponent<Storage>().quantity++;
        }
        else
        { 
            inventory[slot] = ingredient;
            inventory_obj.transform.GetChild(slot).GetComponent<Storage>().AssignIngredient(inventory[slot], slot);
            inventory_obj.transform.GetChild(slot).GetComponent<Storage>().quantity = 1;
        }
    }

    public Ingredient GetIngredientAtSlot(int slot)
    {
        return inventory[slot];
    }

    public void RemoveIngredientAtSlot(int slot)
    {
        Ingredient empty = ScriptableObject.CreateInstance<Ingredient>();

        inventory[slot] = empty;
    }

    public void SwapIngredientLocs(int slot1, int slot2, Ingredient ing1, Ingredient ing2)
    {
        int quantity1 = inventory_obj.transform.GetChild(slot1).GetComponent<Storage>().quantity;
        int quantity2 = inventory_obj.transform.GetChild(slot2).GetComponent<Storage>().quantity;

        AddToInventoryAtSlot(slot1, ing2);
        AddToInventoryAtSlot(slot2, ing1);

        inventory_obj.transform.GetChild(slot1).GetComponent<Storage>().quantity = quantity2;
        inventory_obj.transform.GetChild(slot2).GetComponent<Storage>().quantity = quantity1;
    }

    public bool CompareIDs(int[] ids1, int[] ids2)
    {

        for(int i = 0; i < ids1.Length; i++)
        {
            if(ids1[i] != ids2[i])
            {
                return false;
            }
        }

        return true;
    }
}
