using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public string player_name;

    public int money;
    
    private Ingredient[] inventory = new Ingredient[9];

    private void Start()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = ScriptableObject.CreateInstance<Ingredient>();
        }
    }

    public int[] GetIDsAtSlot(int slot)
    {
        return inventory[slot].ids;
    }

    public void AddToInventoryAtSlot(int slot, Ingredient ingredient)
    {
        inventory[slot] = ingredient;
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

    public void SwapIngredientLocs(int slot1, int slot2)
    {
        Ingredient ing1 = inventory[slot1];
        Ingredient ing2 = inventory[slot2];

        inventory[slot1] = ing2;
        inventory[slot2] = ing1;
    }
}
