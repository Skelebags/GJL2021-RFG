﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/// <summary>
/// Handles most of the gameplay logic
/// </summary>
public class GameManager : MonoBehaviour
{
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
        tooltip = GameObject.Find("Tooltip");
        tooltip.GetComponent<Tooltip>().HideTooltip();
    }


    // Update is called once per frame
    void Update()
    {
        // Save data on S
        if (Input.GetKeyDown(KeyCode.S))
        {

            JSONObject data = BuildPlayerData();

            handler.Save(0, data);
        }

        // Load data on L
        if(Input.GetKeyDown(KeyCode.L))
        {
            ApplyLoadData();
        }

    }

    /// <summary>
    /// Create a JSONObject that contains all of the data relevant to the player
    /// </summary>
    /// <returns>The Player Data as a JSONObject</returns>
    JSONObject BuildPlayerData()
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
            if (player.GetIDsAtSlot(i)[0] != 0)
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

        // Return the player data
        return data;
    }

    /// <summary>
    /// Load in the player's data and update the current game state
    /// </summary>
    void ApplyLoadData()
    {
        // Load in the JSON file
        JSONNode load_data = handler.Load(0);

        // Set the player name
        player.player_name = load_data["Save_Name"].Value;

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
                player.AddToInventoryAtSlot(i, generator.GenerateIngredientFromIDs(idArray[0].AsInt, idArray[1].AsInt, idArray[2].AsInt));
                player.SetQuantityAtSlot(i, quantityArray[i].AsInt);
            }
        }
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
}
