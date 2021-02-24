using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public GameObject ingredientHolder;

    private IngredientGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<IngredientGenerator>();

    }


    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            ClearSprites();
            SpawnSprites();
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
