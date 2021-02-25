using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class GameManger : MonoBehaviour
{
    public GameObject ingredientHolder;

    private IngredientGenerator generator;

    private SaveFileHandler handler;

    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<IngredientGenerator>();
        handler = GetComponent<SaveFileHandler>();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            ClearSprites();
            SpawnSprites();
        }


        if (Input.GetKeyDown(KeyCode.S))
        {

            JSONObject data = new JSONObject();
            // Save money
            data.Add("Money", 0);
            // Save
            data.Add("Day", 1);

            // INVENTORY STARTS EMPTY
            JSONArray inventory = new JSONArray();
            for (int i = 0; i < 9; i++)
            {
                JSONObject item = new JSONObject();
                JSONArray idArray = new JSONArray();
                foreach(int id in generator.Ingredients[i].ids)
                {
                    idArray.Add(id);
                }
                item.Add("id", idArray);
                inventory.Add(item);
            }
            data.Add("Inventory", inventory);

            handler.Save(0, data);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            ClearSprites();

            JSONNode load_data = handler.Load(0);

            JSONArray inventoryArray = load_data["Inventory"].AsArray;

            for(int i = 0; i < inventoryArray.Count; i++)
            {
                if (inventoryArray[i]["id"].Value != "none")
                {
                    JSONArray idArray = inventoryArray[i]["id"].AsArray;
                    generator.Ingredients[i] = generator.GenerateIngredientFromIDs(idArray[0].AsInt, idArray[1].AsInt, idArray[2].AsInt);
                }
                else
                {
                    Debug.Log("No saved inventory");
                }
            }
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

    void SpawnSprites()
    {

        for (int i = 0; i < generator.Ingredients.Count; i++)
        {
            GameObject renderedIngredient = Instantiate(ingredientHolder, new Vector3(0, 0, 0), Quaternion.identity);
            renderedIngredient.GetComponent<SpriteRenderer>().sprite = generator.Ingredients[i].sprite;
            renderedIngredient.transform.position = new Vector3(-Camera.main.orthographicSize + (renderedIngredient.GetComponent<SpriteRenderer>().sprite.bounds.size.x * i), 0);
        }
    }
}
