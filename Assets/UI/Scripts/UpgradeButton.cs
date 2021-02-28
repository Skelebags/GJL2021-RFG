using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    private PlayerDataManager player;
    private ShopManager shop;
    public TMPro.TextMeshProUGUI textMesh;

    [SerializeField]
    private int costMult = 1000;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDataManager>();
        shop = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>();
    }

    private void Update()
    {
        textMesh.text = "Upgrade Shop: (" + shop.GetTier() * costMult + "GP)";
    }

    public void Upgrade()
    {
        if(player.CanAfford(shop.GetTier()*costMult))
        {
            player.Purchase(shop.GetTier() * costMult);
            shop.Upgrade();
        }
    }
}
