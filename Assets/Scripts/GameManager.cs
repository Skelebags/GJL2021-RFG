using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/// <summary>
/// Handles most of the gameplay logic
/// </summary>
public class GameManager : MonoBehaviour
{
    // What save slot is this?
    public int saveSlot;

    // The Ingredient Generator in this scene
    private IngredientGenerator generator;

    private SaveFileHandler handler;
    
    private PlayerDataManager player;

    private GameObject tooltip;

    // Start is called before the first frame update
    void Start()
    {
        // Grab all of our components
        generator = GameObject.FindGameObjectWithTag("IngredientGenerator").GetComponent<IngredientGenerator>();
        handler = GetComponent<SaveFileHandler>();
        player = GetComponent<PlayerDataManager>();
        if(tooltip = GameObject.Find("Tooltip"))
        {
            tooltip.GetComponent<Tooltip>().HideTooltip();
        }
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (PersistenceScript.instance == transform.parent.GetComponent<PersistenceScript>())
        {
            // Grab all of our components
            generator = GameObject.FindGameObjectWithTag("IngredientGenerator").GetComponent<IngredientGenerator>();
            handler = GetComponent<SaveFileHandler>();
            player = GetComponent<PlayerDataManager>();
            if (tooltip = GameObject.Find("Tooltip"))
            {
                tooltip.GetComponent<Tooltip>().HideTooltip();
            }

            player.RefreshInventory();
        }
    }


    /// <summary>
    /// Create a JSONObject that contains all of the data relevant to the player
    /// </summary>
    /// <returns>The Player Data as a JSONObject</returns>
    public JSONObject BuildPlayerData()
    {
        JSONObject data = new JSONObject();

        // Save player name
        data.Add("Save_Name", player.player_name);

        // Save money
        data.Add("Money", player.money);
        // Save day
        data.Add("Day", 1);

        // Save current inventory
        JSONArray invArray = new JSONArray();
        for (int i = 0; i < 9; i++)
        {
            JSONObject item = new JSONObject();
            // If there is an item in this inventory slot, save it to an array of ids
            int[] ids = player.GetIDsAtSlot(i);

            if (ids[0] != 0)
            {
                JSONArray idArray = new JSONArray();
                foreach (int id in player.GetIDsAtSlot(i))
                {
                    idArray.Add(id);
                }
                item.Add("id", idArray);
            }
            else
            {
                item.Add("id", "none");
            }
            // Add the item to the inventory array
            invArray.Add(item);
        }
        // Save the inventory array
        data.Add("Inventory", invArray);

        // Save the quantities of each item in the inventory slots
        JSONArray quantities = new JSONArray();
        for (int i = 0; i < 9; i++)
        {
            quantities.Add("slot " + i, player.GetQuantityAtSlot(i));
        }
        data.Add("Quantities", quantities);

        // Save the shop tiers
        JSONArray tiers = new JSONArray();

        tiers.Add(PartList.Part.Tags.plant.ToString(), player.GetTierFromTag(PartList.Part.Tags.plant));

        tiers.Add(PartList.Part.Tags.meat.ToString(), player.GetTierFromTag(PartList.Part.Tags.meat));
        
        tiers.Add(PartList.Part.Tags.mineral.ToString(), player.GetTierFromTag(PartList.Part.Tags.mineral));

        data.Add("Shop_Tiers", tiers);


        // Return the player data
        return data;
    }

    /// <summary>
    /// Load in the player's data and update the current game state
    /// </summary>
    public void ApplyLoadData()
    {
        // Load in the JSON file
        JSONNode load_data = handler.Load(saveSlot);

        // Set the player name
        player.player_name = load_data["Save_Name"].Value;
        player.day = load_data["Day"].AsInt;
        player.money = load_data["Money"].AsInt;

        // Grab the inventory and quantity arrays
        JSONArray inventoryArray = load_data["Inventory"].AsArray;
        JSONArray quantityArray = load_data["Quantities"].AsArray;

        // Iterate through and assign the items to their slots, with their quantities
        for (int i = 0; i < inventoryArray.Count; i++)
        {
            if (inventoryArray[i]["id"].Value != "none")
            {
                JSONArray idArray = inventoryArray[i]["id"].AsArray;
                // Tell the generator to create the specific ingredient that corresponds to the stored IDs
                player.SetInventoryAtSlot(i, generator.GenerateIngredientFromIDs(idArray[0].AsInt, idArray[1].AsInt, idArray[2].AsInt), quantityArray[i].AsInt);
                Debug.Log(quantityArray[i].AsInt);
            }
            else
            {
                Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();
                player.SetInventoryAtSlot(i, ingredient, quantityArray[i].AsInt);
            }
        }

        JSONArray tiers = load_data["Shop_Tiers"].AsArray;

        player.SetTierForTag(PartList.Part.Tags.plant, tiers[0].AsInt);
        player.SetTierForTag(PartList.Part.Tags.meat, tiers[1].AsInt);
        player.SetTierForTag(PartList.Part.Tags.mineral, tiers[2].AsInt);
    }

    public void SaveData()
    {
        handler.Save(saveSlot, BuildPlayerData());
    }

    // Returns this scene's tooltip object
    public GameObject GetTooltip()
    {
        return tooltip;
    }

    // Returns the player
    public PlayerDataManager GetPlayer()
    {
        return player;
    }

    public IngredientGenerator GetGenerator()
    {
        return generator;
    }

    public void SetSaveSlot(int newSlot)
    {
        saveSlot = newSlot;
    }
}
