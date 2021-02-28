using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Knight_Customer : Customer, IPointerDownHandler
{
    public override int Sell(Potion potion)
    {
        int sellPrice = 0;

        float floatPrice = 0;

        if(potion.effects_dict["str"] > 0)
        {
            floatPrice += potion.effects_dict["str"] * str_mult;
        }
        if (potion.effects_dict["int"] > 0)
        {
            floatPrice += potion.effects_dict["int"] * int_mult;
        }
        if (potion.effects_dict["dex"] > 0)
        {
            floatPrice += potion.effects_dict["dex"] * dex_mult;
        }
        
        sellPrice = (int)floatPrice;

        if (sellPrice < 0)
        {
            sellPrice = 0;
        }

        hasBeenServed = true;

        return sellPrice;
    }

    new public void OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<AudioManager>().Play("Adventurer1", 1f);
    }
}
