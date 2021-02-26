using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Storage : MonoBehaviour, IPointerDownHandler
{
    private GameObject catcher;
    public GameObject icon_prefab;
    public int quantity;

    private Image storedImage;
    private Ingredient heldIngredient;

    private int slot = 0;

    // Start is called before the first frame update
    void Start()
    {
        catcher = GameObject.FindGameObjectWithTag("Catcher");
        
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
            GameObject draggedIcon = Instantiate(icon_prefab, Input.mousePosition, Quaternion.identity, GameObject.Find("Icons").transform);
            UIElementDragger dragger = draggedIcon.GetComponent<UIElementDragger>();
            dragger.dragging = true; /*dragger.cauldron = cauldron;*/ dragger.spawn = transform.gameObject; dragger.SetIngredient(Instantiate(heldIngredient));
            quantity--;
        }
    }

    public void AssignIngredient(Ingredient ingredient, int slot_number)
    {
        heldIngredient = Instantiate(ingredient);

        //GetComponent<Display_Tooltip>().SetTooltipText(heldIngredient.name);

        GetComponent<Display_Tooltip>().SetTooltipText(heldIngredient.name + "\n" + heldIngredient.desc_string + "\n" + heldIngredient.effect_string);

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
