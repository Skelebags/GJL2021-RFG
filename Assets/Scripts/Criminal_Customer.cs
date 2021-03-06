﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Criminal_Customer : Customer, IPointerDownHandler
{
    public override int Sell(Potion potion)
    {
        int sellPrice = 0;

        float floatPrice = 0;

        if (potion.effects_dict["str"] < 0)
        {
            floatPrice += potion.effects_dict["str"] * str_mult;
        }
        if (potion.effects_dict["int"] < 0)
        {
            floatPrice += potion.effects_dict["int"] * int_mult;
        }
        if (potion.effects_dict["dex"] < 0)
        {
            floatPrice += potion.effects_dict["dex"] * dex_mult;
        }

        sellPrice = (int)floatPrice * -1;

        if (sellPrice < 0)
        {
            sellPrice = 0;
        }

        hasBeenServed = true;

        return sellPrice;
    }

    new public void OnPointerDown(PointerEventData eventData)
    {
        if(!isVisible)
        {
            FindObjectOfType<AudioManager>().Play("Assassin", 1f);
            isVisible = true;
        }
    }
}
