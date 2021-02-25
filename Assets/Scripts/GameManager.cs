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

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    ClearSprites();
        //    SpawnSprites();
        //}

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    ClearSprites();
        //    SpawnInventory();
        //}


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

        //if(Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    for(int i = 0; i < 3; i++)
        //    {
        //        player.AddToInventoryAtSlot(i, generator.Ingredients[i]);
        //    }
        //}
    }

    void ClearSprites()
    {
        GameObject[] sprites = GameObject.FindGameObjectsWithTag("Ingredient");

        foreach(GameObject sprite in sprites)
        {
            Destroy(sprite);
        }
    }

    //void SpawnSprites()
    //{

    //    for (int i = 0; i < generator.Ingredients.Count; i++)
    //    {
    //        GameObject renderedIngredient = Instantiate(ingredientHolder, new Vector3(0, 0, 0), Quaternion.identity);
    //        renderedIngredient.GetComponent<SpriteRenderer>().sprite = generator.Ingredients[i].sprite;
    //        renderedIngredient.transform.position = new Vector3(-Camera.main.orthographicSize + (renderedIngredient.GetComponent<SpriteRenderer>().sprite.bounds.size.x * i), 0);
    //    }
    //}

    //void SpawnInventory()
    //{

    //    for (int i = 0; i < 9; i++)
    //    {
    //        if(player.GetIDsAtSlot(i)[0] != 0)
    //        {
    //            GameObject renderedIngredient = Instantiate(ingredientHolder, new Vector3(0, 0, 0), Quaternion.identity);
    //            renderedIngredient.GetComponent<SpriteRenderer>().sprite = player.GetIngredientAtSlot(i).sprite;
    //            renderedIngredient.transform.position = new Vector3(-Camera.main.orthographicSize + (renderedIngredient.GetComponent<SpriteRenderer>().sprite.bounds.size.x * i), 0);
    //        }
    //    }
    //}

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

        return data;
    }

    void ApplyLoadData()
    {
        JSONNode load_data = handler.Load(0);

        player.player_name = load_data["Save_Name"].Value;

        JSONArray inventoryArray = load_data["Inventory"].AsArray;

        for (int i = 0; i < inventoryArray.Count; i++)
        {
            if (inventoryArray[i]["id"].Value != "none")
            {
                JSONArray idArray = inventoryArray[i]["id"].AsArray;
                //generator.Ingredients[i] = generator.GenerateIngredientFromIDs(idArray[0].AsInt, idArray[1].AsInt, idArray[2].AsInt);
                player.AddToInventoryAtSlot(i, generator.GenerateIngredientFromIDs(idArray[0].AsInt, idArray[1].AsInt, idArray[2].AsInt));
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
}
