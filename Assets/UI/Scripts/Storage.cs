﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Storage : MonoBehaviour, IPointerDownHandler
{
    private GameManager manager;
    public GameObject icon_prefab;
    public int quantity;

    private Image storedImage;
    private Ingredient heldIngredient;

    private int slot = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Text>().text = quantity.ToString();
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (quantity >= 1)
        {
            if (transform.parent.CompareTag("Inventory") || manager.GetPlayer().CanAfford(heldIngredient.cost))
            {
                GameObject draggedIcon = Instantiate(icon_prefab, Input.mousePosition, Quaternion.identity, GameObject.Find("Icons").transform);
                UIElementDragger dragger = draggedIcon.GetComponent<UIElementDragger>();
                dragger.dragging = true; dragger.spawn = transform.gameObject; dragger.SetIngredient(/*Instantiate(heldIngredient)*/heldIngredient); dragger.manager = manager;
                quantity--;
            }
        }
    }

    public void AssignIngredient(Ingredient ingredient, int slot_number)
    {
        //heldIngredient = Instantiate(ingredient);
        heldIngredient = ingredient;

        GetComponent<Display_Tooltip>().SetTooltipText(heldIngredient.name + "\n" + "Price: " + heldIngredient.cost + "\n" + heldIngredient.desc_string + "\n" + heldIngredient.effect_string);

        transform.Find("StoredImage").GetComponent<Image>().sprite = heldIngredient.sprite;

        slot = slot_number;
    }

    public bool IsEmpty()
    {
        return (quantity == 0) ;
    }

    public int GetSlotNumber()
    {
        return slot;
    }

    public Ingredient GetIngredient()
    {
        return heldIngredient;
    }
}
