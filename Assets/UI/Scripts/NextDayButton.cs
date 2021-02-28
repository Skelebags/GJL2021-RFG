using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayButton : MonoBehaviour
{
    private PlayerDataManager player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>();
    }

    public void NextDay()
    {
        player.NextDay();
    }
}
