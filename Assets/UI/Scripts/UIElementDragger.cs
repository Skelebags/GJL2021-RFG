using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : MonoBehaviour
{

    public bool dragging;
    //public GameObject cauldron;
    private GameObject overlap = null;
    public GameObject spawn;
    private bool returning = false;
    public float returnSpeed = 0.5f;

    private Ingredient ingredient;

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                //if (RectTransformOverlap(GetComponent<RectTransform>(), cauldron.GetComponent<RectTransform>()))
                //{
                    
                //}
                //else if(RectTransformOverlap(GetComponent<RectTransform>(), ))
                //{

                //}
                //else
                //{
                //    returning = true;
                //}

                if(overlap == null)
                {
                    returning = true;
                }
                else if(overlap.CompareTag("Catcher"))
                {
                    if(spawn.transform.parent.name == "Inventory")
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        returning = true;
                    }
                }
                else if(overlap.transform.parent.name == "Inventory")
                {
                    PlayerDataManager playerDataManager = GameObject.Find("GameManager").GetComponent<PlayerDataManager>();

                    if (overlap.GetComponent<Storage>().IsEmpty() || playerDataManager.CompareIDs(ingredient.ids, overlap.GetComponent<Storage>().GetIngredient().ids))
                    {
                        GameObject.Find("GameManager").GetComponent<PlayerDataManager>().AddToInventoryAtSlot(overlap.GetComponent<Storage>().GetSlotNumber(), ingredient);
                        Destroy(gameObject);
                    }
                    else if(spawn.transform.parent.name == "Inventory")
                    {
                        GameObject.Find("GameManager").GetComponent<PlayerDataManager>().SwapIngredientLocs(spawn.GetComponent<Storage>().GetSlotNumber(), overlap.GetComponent<Storage>().GetSlotNumber(), ingredient, overlap.GetComponent<Storage>().GetIngredient());
                        overlap.GetComponent<Storage>().quantity++;
                        Destroy(gameObject);
                    }
                    else
                    {
                        returning = true;
                    }
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
                spawn.GetComponent<Storage>().AssignIngredient(ingredient, spawn.GetComponent<Storage>().GetSlotNumber());
                spawn.GetComponent<Storage>().quantity++;
                Destroy(transform.gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided!");
        overlap = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Leaving");
        overlap = null;
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

    public void SetIngredient(Ingredient newIngredient)
    {
        ingredient = Instantiate(newIngredient);
        GetComponent<Image>().sprite = ingredient.sprite;
        GetComponent<Display_Tooltip>().SetTooltipText(ingredient.name + "\n" + "Price: " + ingredient.cost + "\n" + ingredient.desc_string + "\n" + ingredient.effect_string);
    }
}
