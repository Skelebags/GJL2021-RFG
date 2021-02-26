using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Display_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameManager manager;
    private GameObject tooltip;
    private string tooltipText = "";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(tooltip)
        {
            tooltip.GetComponent<Tooltip>().ShowTooltip(tooltipText);
            //Debug.Log(tooltipText);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        tooltip.GetComponent<Tooltip>().HideTooltip();
    }

    public void SetTooltipText(string TooltipTextInput)
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tooltip = manager.GetTooltip();

        tooltipText = TooltipTextInput;

    }
}
