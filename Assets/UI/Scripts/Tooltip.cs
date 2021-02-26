using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    
    [SerializeField]
    private Camera uiCamera;
    
    private Text tooltipTextComp;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        if(backgroundRectTransform == null)
        {
            //backgroundRectTransform = transform.Find("Background").gameObject.GetComponent<RectTransform>();
        }
        if(tooltipTextComp == null)
        {
            tooltipTextComp = GetComponentInChildren<Text>();//transform.Find("TooltipText").gameObject.GetComponent<Text>();
        }
        //ShowTooltip("Test Text");
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }
    
    public void ShowTooltip(string tooltipString)
    {        
        tooltipTextComp.text = tooltipString;
        tooltipTextComp.supportRichText = true;
        if(tooltipTextComp.text.Substring(0, 1) != "\n" )
        {
            gameObject.SetActive(true);
        }
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

   



}
