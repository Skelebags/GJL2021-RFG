using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public GameObject newGameScreen;
    public Navigator navigator;

    private PlayerDataManager player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>();
    }

    public void NewOrLoad()
    {
        if(player.player_name == "NO SAVE DATA")
        {
            newGameScreen.SetActive(true);
        }
        else
        {
            navigator.ToMap();
        }
    }
}
