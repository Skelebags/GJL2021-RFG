using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour, IPointerDownHandler
{
    public GameObject icon_prefab;

    private GameObject fillObj;
    private Color fillColour;
    private Image imageComponent;
    private List<Ingredient> contents = new List<Ingredient>();

    public Dictionary<string, float> effects_dict = new Dictionary<string, float>() { { "str", 0f }, {"int", 0f }, {"dex", 0f } };

    private string effect_string = "";


    void Start()
    {
        fillObj = transform.Find("Fill").gameObject;
        fillColour = fillObj.GetComponent<Image>().color;
    }

    void Update()
    {
        if(contents.Count > 0)
        {
            fillObj.SetActive(true);
        }
        else
        {
            fillObj.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (contents.Count >= 1)
        {
            Potion newPotion = ScriptableObject.CreateInstance<Potion>();
            newPotion.effects_dict = effects_dict;
            newPotion.effect_string = effect_string;

            GameObject draggedIcon = Instantiate(icon_prefab, Input.mousePosition, Quaternion.identity, GameObject.Find("Icons").transform);
            UIElementDragger dragger = draggedIcon.GetComponent<UIElementDragger>();
            dragger.dragging = true; dragger.spawn = transform.gameObject; dragger.SetPotion(newPotion);
            EmptyCauldron();
        }
    }

    public void AddIngredientToCauldron(Ingredient ingredient)
    {
        if (!contents.Contains(ingredient))
        {
            contents.Add(ingredient);

            CalculateEffects();
            effect_string = BuildEffectString();
            GetComponent<Display_Tooltip>().SetTooltipText(effect_string);
            fillObj.GetComponent<Image>().color = BuildFillColour();
        }
        else
        {
            Debug.Log("Duplicate!");
        }
    }

    private void CalculateEffects()
    {
        effects_dict["str"] = 0f;
        effects_dict["int"] = 0f;
        effects_dict["dex"] = 0f;
        foreach (Ingredient ingredient in contents)
        {
            Debug.Log(ingredient.effectsDict["str"][0]);
            effects_dict["str"] += ingredient.effectsDict["str"][0];
            Debug.Log(ingredient.effectsDict["int"][0]);
            effects_dict["int"] += ingredient.effectsDict["int"][0];
            Debug.Log(ingredient.effectsDict["dex"][0]);
            effects_dict["dex"] += ingredient.effectsDict["dex"][0];
        }
        foreach (Ingredient ingredient in contents)
        {
            effects_dict["str"] *= ingredient.effectsDict["str"][1];
            effects_dict["int"] *= ingredient.effectsDict["int"][1];
            effects_dict["dex"] *= ingredient.effectsDict["dex"][1];
        }

    }

    private void EmptyCauldron()
    {
        contents.Clear();
        fillObj.GetComponent<Image>().color = Color.white;
        effect_string = "";
        GetComponent<Display_Tooltip>().SetTooltipText(effect_string);
    }

    private Color BuildFillColour()
    {
        Color newColour = fillColour;

        foreach(Ingredient ingredient in contents)
        {
            newColour *= ingredient.sprite.texture.GetPixel(64, 64);
        }

        return newColour;
    }

    private string BuildEffectString()
    {
        string finalString = "";

        string str_string = "";
        string int_string = "";
        string dex_string = "";
        
        if (effects_dict["str"] != 0f)
        {
            str_string = "<color=red>STR: ";
           
            str_string += ((effects_dict["str"] < 0) ? "" : "+") + effects_dict["str"];
            
            str_string += "</color> ";
        }
        if (effects_dict["int"] != 0f)
        {
            int_string = "<color=blue>INT: ";

            int_string += ((effects_dict["int"] < 0) ? "" : "+") + effects_dict["int"];

            int_string += "</color> ";
        }
        if (effects_dict["dex"] != 0f)
        {
            dex_string = "<color=green>DEX: ";

            dex_string += ((effects_dict["dex"] < 0) ? "" : "+") + effects_dict["dex"];

            dex_string += "</color>";
        }

        finalString = str_string + int_string + dex_string;

        return finalString;
    }
}
