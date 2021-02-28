using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Customer : MonoBehaviour, IPointerDownHandler
{

    // How much this customer wants each stat
    public float str_mult = 1.0f;
    public float int_mult = 1.0f;
    public float dex_mult = 1.0f;
    
    // Pitch of voice and array for selecting random voice line.
    public float voicePitch = 1.0f;
    private string[] voiceLine = { "Adventurer1", "Adventurer2", "Adventurer3" };
    
    public bool hasBeenServed = false;

    public GameObject speechBubble;
    private float timer = 0f;
    [SerializeField]
    private float visTime = 5f;
    protected bool isVisible = false;

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

    private void Update()
    {
        if(isVisible)
        {
            timer += Time.deltaTime;
        }

        if(timer >= visTime)
        {
            timer = 0;
            isVisible = false;
        }

        speechBubble.SetActive(isVisible);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isVisible)
        {
            int randomIndex = Random.Range(0, 3);
            string randomVoiceline = voiceLine[randomIndex];
            FindObjectOfType<AudioManager>().Play(randomVoiceline, voicePitch);

            isVisible = true;
        }
    }
}
