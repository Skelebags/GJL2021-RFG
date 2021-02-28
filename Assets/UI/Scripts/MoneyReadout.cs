using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyReadout : MonoBehaviour
{
    private PlayerDataManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = player.GetMoney() + "GP";
    }
}
