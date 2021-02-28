using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{

    // How much this customer wants each stat
    public float str_mult = 1.0f;
    public float int_mult = 1.0f;
    public float dex_mult = 1.0f;

    public bool hasBeenServed = false;

    // Calculate the sell price of a potion based on how much the customer wants it
    public virtual int Sell(Potion potion)
    {
        int sellPrice = 0;

        float floatPrice = 0;

        floatPrice += potion.effects_dict["str"] * str_mult;
        floatPrice += potion.effects_dict["int"] * int_mult;
        floatPrice += potion.effects_dict["dex"] * dex_mult;

        sellPrice = (int)floatPrice;

        if(sellPrice < 0)
        {
            sellPrice = 0;
        }

        hasBeenServed = true;

        return sellPrice;
    }
}
