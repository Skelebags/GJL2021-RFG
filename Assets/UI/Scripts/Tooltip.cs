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
    private RectTransform taper;

    private void Awake()
    {
        if(backgroundRectTransform == null)
        {
            backgroundRectTransform = transform.Find("TooltipSizer").gameObject.GetComponent<RectTransform>();
        }
        if(tooltipTextComp == null)
        {
            tooltipTextComp = GetComponentInChildren<Text>(); //transform.Find("TooltipText").gameObject.GetComponent<Text>();
        }
        if (taper == null)
        {
            taper = transform.Find("Image").GetComponent<RectTransform>(); //transform.Find("TooltipText").gameObject.GetComponent<Text>();
        }

        //ShowTooltip("Test Text");
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
        taper.sizeDelta = new Vector2(taper.sizeDelta.x, backgroundRectTransform.sizeDelta.y);
    }
    
    public void ShowTooltip(string tooltipString)
    {        
        tooltipTextComp.text = tooltipString;
        tooltipTextComp.supportRichText = true;
        
        if(tooltipTextComp.text.Length > 20)
        {
            gameObject.SetActive(true);
        }
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

   



}
