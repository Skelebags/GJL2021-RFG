using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Storage : MonoBehaviour, IPointerDownHandler
{
    private GameObject cauldron;
    public GameObject icon_prefab;
    public int quantity;

    private Image storedImage;
    private Ingredient heldIngredient;

    // Start is called before the first frame update
    void Start()
    {
        cauldron = GameObject.FindGameObjectWithTag("Catcher");
        
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
            dragger.dragging = true; dragger.cauldron = cauldron; dragger.spawn = transform.gameObject; dragger.SetIngredient(Instantiate(heldIngredient));
            quantity--;
        }
    }

    public void AssignIngredient(Ingredient ingredient)
    {
        heldIngredient = Instantiate(ingredient);

        GetComponent<Display_Tooltip>().SetTooltipText(heldIngredient.name);

        transform.Find("StoredImage").GetComponent<Image>().sprite = heldIngredient.sprite;
    }
}
