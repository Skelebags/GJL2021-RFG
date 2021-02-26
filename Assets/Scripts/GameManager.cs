using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class GameManager : MonoBehaviour
{
    public GameObject ingredientHolder;

    private IngredientGenerator generator;

    private SaveFileHandler handler;
    
    private PlayerDataManager player;

    private GameObject tooltip;

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.Find("IngredientGenerator").GetComponent<IngredientGenerator>();
        handler = GetComponent<SaveFileHandler>();
        player = GetComponent<PlayerDataManager>();
        tooltip = GameObject.Find("Tooltip");
        tooltip.GetComponent<Tooltip>().HideTooltip();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {

            JSONObject data = BuildPlayerData();

            handler.Save(0, data);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            ClearSprites();
            ApplyLoadData();
        }

    }

    void ClearSprites()
    {
        GameObject[] sprites = GameObject.FindGameObjectsWithTag("Ingredient");

        foreach(GameObject sprite in sprites)
        {
            Destroy(sprite);
        }
    }

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

            invArray.Add(item);
        }
        data.Add("Inventory", invArray);

        JSONArray quantities = new JSONArray();
        for (int i = 0; i < 9; i++)
        {
            quantities.Add("slot " + i, player.GetQuantityAtSlot(i));
        }
        data.Add("Quantities", quantities);

        return data;
    }

    void ApplyLoadData()
    {
        JSONNode load_data = handler.Load(0);

        player.player_name = load_data["Save_Name"].Value;

        JSONArray inventoryArray = load_data["Inventory"].AsArray;
        JSONArray quantityArray = load_data["Quantities"].AsArray;

        for (int i = 0; i < inventoryArray.Count; i++)
        {
            if (inventoryArray[i]["id"].Value != "none")
            {
                JSONArray idArray = inventoryArray[i]["id"].AsArray;
                player.AddToInventoryAtSlot(i, generator.GenerateIngredientFromIDs(idArray[0].AsInt, idArray[1].AsInt, idArray[2].AsInt));
                player.SetQuantityAtSlot(i, quantityArray[i].AsInt);
            }
            else
            {
                Debug.Log("No saved inventory");
            }
        }
    }

    public GameObject GetTooltip()
    {
        return tooltip;
    }

    public PlayerDataManager GetPlayer()
    {
        return player;
    }
}
