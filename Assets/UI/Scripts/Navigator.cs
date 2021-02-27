using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    public void ToMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void ToPlant_ShopScene()
    {
        SceneManager.LoadScene("Plant_ShopScene");
    }

    public void ToMeat_ShopScene()
    {
        SceneManager.LoadScene("Meat_Shopscene");
    }

    public void ToMineral_ShopScene()
    {
        SceneManager.LoadScene("Mineral_Shopscene");
    }

    public void ToMain()
    {
        SceneManager.LoadScene("Main");
    }
}
