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

    private void Start()
    {
        backgroundRectTransform = transform.Find("Background").gameObject.GetComponent<RectTransform>();
        tooltipTextComp = transform.Find("TooltipText").gameObject.GetComponent<Text>();
        //HideTooltip();

    }
    
    private void Awake()
    {
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
        gameObject.SetActive(true);
        tooltipTextComp.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipTextComp.preferredWidth + textPaddingSize * 2f, tooltipTextComp.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

   



}
