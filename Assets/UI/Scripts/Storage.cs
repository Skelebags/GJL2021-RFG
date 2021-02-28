using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// An object that can store an ingredient, display it's held ingredient, and spawn a draggable ui element when clicked
/// </summary>
public class Storage : MonoBehaviour, IPointerDownHandler
{
    // The draggable object prefab
    public GameObject icon_prefab;

    // The quantity of ingredients present in this storage
    public int quantity;

    // Variable for holding the GameManager, the image component that will display the held ingredient, and the held ingredient itself
    protected GameManager manager;
    private Image storedImage;
    private Ingredient heldIngredient;

    // Which position this storage slot occupies in the inventory
    private int slot = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Grab the game manager
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        // Keep the colliders in check with UI scaling
        GetComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, gameObject.GetComponent<RectTransform>().sizeDelta.y);

        // Keep tabs on the quantity of ingredients available
        if (quantity > 0)
        {
            GetComponentInChildren<TMPro.TextMeshProUGUI>().text = quantity.ToString();
            transform.Find("StoredImage").GetComponent<Image>().sprite = heldIngredient.sprite;
        }
        else
        {
            GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
            transform.Find("StoredImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Empty");
        }

    }

    // When the storage is clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        // If there is any of the ingredient left in the storage
        if (quantity >= 1)
        {
            // If the storage is part of the players inventory *OR* the player can afford to purchase the ingredient from the shop
            if (transform.parent.CompareTag("Inventory") || manager.GetPlayer().CanAfford(heldIngredient.cost))
            {
                FindObjectOfType<AudioManager>().Play("IngredientPickup", 1f);
                // Spawn the draggable element
                GameObject draggedIcon = Instantiate(icon_prefab, Input.mousePosition, Quaternion.identity, GameObject.Find("Icons").transform);
                UIElementDragger dragger = draggedIcon.GetComponent<UIElementDragger>();
                dragger.dragging = true; dragger.spawn = transform.gameObject; dragger.SetIngredient(/*Instantiate(heldIngredient)*/heldIngredient); dragger.manager = manager;
                // Decrement the remaining quantity of ingredients
                quantity--;
            }
        }
    }

    /// <summary>
    /// Set the ingredient held in this storage, and its storage slot number
    /// </summary>
    /// <param name="ingredient">The ingredient to set</param>
    /// <param name="slot_number">The slot number to set</param>
    public void AssignIngredient(Ingredient ingredient, int slot_number)
    {
        heldIngredient = ingredient;

        GetComponent<Display_Tooltip>().SetTooltipText(heldIngredient.ingredient_name + "\n" + "Price: " + heldIngredient.cost + "\n" + heldIngredient.desc_string + "\n" + heldIngredient.effect_string);

        transform.Find("StoredImage").GetComponent<Image>().sprite = heldIngredient.sprite;
        slot = slot_number;
    }

    // Does the storage slot have anything left in it
    public bool IsEmpty()
    {
        return (quantity == 0) ;
    }

    // Returns the storage's slot number
    public int GetSlotNumber()
    {
        return slot;
    }

    // Returns the held ingredient
    public Ingredient GetIngredient()
    {
        return heldIngredient;
    }
}
