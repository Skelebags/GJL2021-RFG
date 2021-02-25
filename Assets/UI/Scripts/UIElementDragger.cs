using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : MonoBehaviour
{

    public bool dragging;
    public GameObject cauldron;
    public GameObject spawn;
    private bool returning = false;
    public float returnSpeed = 0.5f;

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                if (RectTransformOverlap(GetComponent<RectTransform>(), cauldron.GetComponent<RectTransform>()))
                {
                 
                }
                else
                {
                    returning = true;
                }
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (returning)
        {
            transform.position = Vector2.Lerp(transform.position, spawn.transform.position, returnSpeed);
            if (RectTransformOverlap(GetComponent<RectTransform>(), spawn.GetComponent<RectTransform>()))
            {
                returning = false;
                spawn.GetComponent<Storage>().quantity++;
                Destroy(transform.gameObject);
            }
        }
    }

    public void Awake()
    {

    }


    public bool RectTransformOverlap(RectTransform rectTransform1, RectTransform rectTransform2)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform1.GetWorldCorners(corners);
        Rect rec = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        rectTransform2.GetWorldCorners(corners);
        Rect rec2 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        return rec.Overlaps(rec2);


    }
}
