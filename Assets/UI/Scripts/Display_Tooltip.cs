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

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(tooltip = GameObject.Find("GameManager").GetComponent<GameManager>().GetTooltip())
        {
            tooltip.GetComponent<Tooltip>().ShowTooltip(tooltipText);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(tooltip = GameObject.Find("GameManager").GetComponent<GameManager>().GetTooltip())
        {
            tooltip.GetComponent<Tooltip>().HideTooltip();
        }
    }

    public void SetTooltipText(string TooltipTextInput)
    {

        tooltipText = TooltipTextInput;

    }

    public void ForceHideTooltip()
    {
        if(tooltip = GameObject.Find("GameManager").GetComponent<GameManager>().GetTooltip())
        {
            tooltip.GetComponent<Tooltip>().HideTooltip();
        }
    }
    
}
