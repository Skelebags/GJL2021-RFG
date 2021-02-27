using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Attaching this script to an object causes it to display a tooltip when hovered over
/// </summary>
public class Display_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Variables to hold relevant objects
    private GameObject tooltip;

    // The string that will be displayed on the tooltip
    private string tooltipText = "";

    // When hovered over
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // If we can find a tooltip object from the game manager
        if(tooltip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetTooltip())
        {
            // Show the tooltip
            tooltip.GetComponent<Tooltip>().ShowTooltip(tooltipText);
        }
    }

    // When stop hovering over
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // If we can find a tooltip object from the game manager
        if (tooltip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetTooltip())
        {
            // Hide the tooltip
            tooltip.GetComponent<Tooltip>().HideTooltip();
        }
    }

    /// <summary>
    /// Set this object's tooltip text
    /// </summary>
    /// <param name="TooltipTextInput">The desired text</param>
    public void SetTooltipText(string TooltipTextInput)
    {

        tooltipText = TooltipTextInput;

    }

    /// <summary>
    /// Force the tooltip to be hidden
    /// </summary>
    public void ForceHideTooltip()
    {
        // If we can find a tooltip object from the game manager
        if (tooltip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetTooltip())
        {
            // Hide the tooltip
            tooltip.GetComponent<Tooltip>().HideTooltip();
        }
    }
    
}
