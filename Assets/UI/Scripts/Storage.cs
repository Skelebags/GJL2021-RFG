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
            draggedIcon.GetComponent<UIElementDragger>().dragging = true; draggedIcon.GetComponent<UIElementDragger>().cauldron = cauldron; draggedIcon.GetComponent<UIElementDragger>().spawn = transform.gameObject;
            quantity--;
        }
    }

    
}
