using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A draggable UI Element
/// </summary>
public class UIElementDragger : MonoBehaviour, IPointerDownHandler
{
    
    // The object that Instantiated this element
    public GameObject spawn;

    // This scene's game manager
    public GameManager manager;
    
    // What was the most recent thing the element has overlapped
    private GameObject overlap = null;


     // Are we being dragged
    public bool dragging;
    // Is the element returning to spawn
    private bool returning = false;
    // The speed to return to spawn
    public float returnSpeed = 0.5f;

    // What ingredient/potion does this element represent
    private Ingredient ingredient;
    private Potion potion;

    // How many things is the element overlapping
    //private int overlaps = 0;
    private List<Collider2D> overlaps = new List<Collider2D>();

    public void Update()
    {
        // If the element is being dragged
        if (dragging)
        {
            // Follow the mouse position
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // If the mouse lets go
            if (Input.GetMouseButtonUp(0))
            {
                // The element is no longer being dragged
                dragging = false;

                overlap = GetClosestOverlap();

                // Grab the player data manager for ease
                PlayerDataManager playerDataManager = manager.GetPlayer();

                // If this element is an ingredient
                if (ingredient)
                {

                    // If nothing is being overlapped
                    if (overlap == null)
                    {
                        // Return to spawn
                        returning = true;
                    }
                    // If the element overlaps with a "Catcher"
                    else if (overlap.CompareTag("Catcher"))
                    {
                        // Check if the origin was the player's inventory (We don't want the player to sell stuff to the shop from the shop's inventory, for example)
                        if (spawn.transform.parent.CompareTag("Inventory") || spawn.transform.CompareTag("Generic"))
                        {
                            // If the overlap is a cauldron
                            if (overlap.GetComponent<Cauldron>())
                            {
                                // Add the ingredient to the cauldron
                                overlap.GetComponent<Cauldron>().AddIngredientToCauldron(ingredient);
                            }
                            // Otherwise it must be the sell field of a shop
                            else
                            {
                                playerDataManager.AddMoney(ingredient.cost / 2);
                            }
                            // Cleanup the element
                            Cleanup();
                        }
                        else
                        {
                            // Return to spawn
                            returning = true;
                        }
                    }
                    // If the element overlaps with an inventory slot
                    else if (overlap.transform.parent.CompareTag("Inventory"))
                    {
                        // If the overlapping storage is empty, or the overlapped storage already contains this ingredient
                        if (overlap.GetComponent<Storage>().IsEmpty() || playerDataManager.CompareIDs(ingredient.ids, overlap.GetComponent<Storage>().GetIngredient().ids))
                        {
                            if (!spawn.transform.CompareTag("Generic"))
                            {
                                // Add it to the inventory slot
                                playerDataManager.AddToInventoryAtSlot(overlap.GetComponent<Storage>().GetSlotNumber(), ingredient);

                                // If the element is from the shop
                                if (spawn.transform.parent.CompareTag("Shop"))
                                {
                                    // PAY FOR IT!!!
                                    playerDataManager.Purchase(ingredient.cost);
                                }
                                // Cleanup the element
                                Cleanup();
                            }
                            else
                            {
                                returning = true;
                            }
                        }
                        // If the overlapping storage is not empty, and does not contain this ingredient, but is still an inventory slot
                        else if (spawn.transform.parent.name == "Inventory")
                        {
                            // Swap round the items!
                            playerDataManager.SwapIngredientLocs(spawn.GetComponent<Storage>().GetSlotNumber(), overlap.GetComponent<Storage>().GetSlotNumber(), ingredient, overlap.GetComponent<Storage>().GetIngredient());

                            // Account for the decrement on pickup
                            overlap.GetComponent<Storage>().quantity++;

                            // Cleanup the element
                            Cleanup();
                        }
                        else
                        {
                            // Get back to spawn
                            returning = true;
                        }
                    }
                    else
                    {
                        // Get back to spawn
                        returning = true;
                    }
                }
                else if(potion)
                {
                    if(overlap)
                    {
                        if (overlap.CompareTag("Customer"))
                        {
                            playerDataManager.AddMoney(overlap.GetComponent<Customer>().Sell(potion));
                            Cleanup();
                        }
                    }

                }
                
            }
        }
        
    }

    private void FixedUpdate()
    {
        // While an element is returning
        if (returning)
        {
            // Lerp it back to its spawn point
            transform.position = Vector2.Lerp(transform.position, spawn.transform.position, returnSpeed);

            // Once it overlaps with it's spawning storage object
            if (RectTransformOverlap(GetComponent<RectTransform>(), spawn.GetComponent<RectTransform>()))
            {
                // It's made it back
                returning = false;
                
                // Put it back in the box
                spawn.GetComponent<Storage>().AssignIngredient(ingredient, spawn.GetComponent<Storage>().GetSlotNumber());
                spawn.GetComponent<Storage>().quantity++;

                // Do the cleanup
                Cleanup();
            }
        }
    }

    // When you click on an existing element (Pretty much only potions)
    public void OnPointerDown(PointerEventData eventData)
    {
        // start dragging it
        dragging = true;
    }

    /// <summary>
    /// Destroys the element and force hides the tooltip
    /// </summary>
    private void Cleanup()
    {
        GetComponent<Display_Tooltip>().ForceHideTooltip();
        Destroy(gameObject);
    }

    // When the element enters another trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Keep track of what it's overlapped most recently
        //overlap = collision.gameObject;

        // and how many things it is overlapping with
        //overlaps++;
        overlaps.Add(collision);
    }

    // When the element leaves a trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        // It's overlapping one less thing
        overlaps.Remove(collision);
        
        // If it's overlapping nothing, set overlap to null
        if(overlaps.Count <= 0)
        {
            overlaps.Clear();

            //overlap = null;
        }
    }

    /// <summary>
    /// Checks if two RectTransforms overlap
    /// </summary>
    /// <param name="rectTransform1">The First RectTransform</param>
    /// <param name="rectTransform2">The Second RectTransform</param>
    /// <returns>Whether they are overlapping or not</returns>
    public bool RectTransformOverlap(RectTransform rectTransform1, RectTransform rectTransform2)
    {
        // Build a Rect from the world corners of each RectTransform
        Vector3[] corners = new Vector3[4];
        rectTransform1.GetWorldCorners(corners);
        Rect rec = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        rectTransform2.GetWorldCorners(corners);
        Rect rec2 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        // Check if the Rects overlap
        return rec.Overlaps(rec2);
    }

    // Set the element's ingredient and tooltip data
    public void SetIngredient(Ingredient newIngredient)
    {
        ingredient = newIngredient;
        GetComponent<Image>().sprite = ingredient.sprite;
        GetComponent<Display_Tooltip>().SetTooltipText(ingredient.ingredient_name + "\n" + "Price: " + ingredient.cost + "\n" + ingredient.desc_string + "\n" + ingredient.effect_string);
    }

    // Set the element's potion and tooltip data
    public void SetPotion(Potion newPotion)
    {
        potion = newPotion;
        GetComponent<Image>().sprite = potion.sprite;
        GetComponent<Display_Tooltip>().SetTooltipText(potion.effect_string);
        transform.localScale *= 1.5f;
    }

    private GameObject GetClosestOverlap()
    {
        if(overlaps.Count == 0)
        {
            return null;
        }
        else
        {
            GameObject closest = overlaps[0].gameObject;
            foreach(Collider2D collider in overlaps)
            {
                if((collider.transform.position - transform.position).magnitude < (closest.transform.position - transform.position).magnitude)
                {
                    closest = collider.gameObject;
                }
            }
            return closest;
        }
    }
}
