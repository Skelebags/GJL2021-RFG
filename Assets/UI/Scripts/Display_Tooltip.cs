using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Display_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltip;
    private string tooltipText = "Default";
    
    // Start is called before the first frame update
    void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.GetComponent<Tooltip>().HideTooltip();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        tooltip.GetComponent<Tooltip>().ShowTooltip(tooltipText);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        tooltip.GetComponent<Tooltip>().HideTooltip();
    }

    public void SetTooltipText(string TooltipTextInput)
    {
        tooltipText = TooltipTextInput;

    }
}
