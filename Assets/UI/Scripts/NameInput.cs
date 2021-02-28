using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameInput : MonoBehaviour
{
    private PlayerDataManager player;
    public TMPro.TMP_InputField input;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>();
    }

    public void SetPlayerName()
    {
        player.player_name = input.text;
    }
}
