using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SaveReadoutText : MonoBehaviour
{
    public int save_slot;

    private SaveFileHandler handler;
    private TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        handler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveFileHandler>();
        text = GetComponent<TMPro.TextMeshProUGUI>();

        BuildTextFromData();
    }

    public void BuildTextFromData()
    {
        JSONNode data = handler.Load(save_slot);

        string name_string = data["Save_Name"].Value;
        string day_string = data["Day"].AsInt.ToString();
        string money_string = data["Money"].AsInt.ToString();

        string final_string = "Name: " + name_string + "\nDay: " + day_string + "\nMoney: " + money_string;
        text.text = final_string;
    }
}
