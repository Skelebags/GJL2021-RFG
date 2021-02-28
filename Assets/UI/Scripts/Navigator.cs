using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    public void ToMap()
    {
        SceneManager.LoadScene("Map");
        FindObjectOfType<AudioManager>().Play("PaperCrinkle", 1f);
        FindObjectOfType<AudioManager>().Stop("Crowd");
    }

    public void ToPlant_ShopScene()
    {
        SceneManager.LoadScene("Plant_ShopScene");
        FindObjectOfType<AudioManager>().Play("PaperCrinkle", 1f);
        FindObjectOfType<AudioManager>().Play("Crowd", 1f);
    }

    public void ToMeat_ShopScene()
    {
        SceneManager.LoadScene("Meat_Shopscene");
        FindObjectOfType<AudioManager>().Play("PaperCrinkle", 1f);
        FindObjectOfType<AudioManager>().Play("Crowd", 1f);
    }

    public void ToMineral_ShopScene()
    {
        SceneManager.LoadScene("Mineral_Shopscene");
        FindObjectOfType<AudioManager>().Play("PaperCrinkle", 1f);
        FindObjectOfType<AudioManager>().Play("Crowd", 1f);
    }

    public void ToMain()
    {
        SceneManager.LoadScene("Main");
        FindObjectOfType<AudioManager>().Play("PaperCrinkle", 1f);
        FindObjectOfType<AudioManager>().Play("Bubbling”", 1f);
        FindObjectOfType<AudioManager>().Stop("Crowd");
    }

    public void SaveData()
    {
        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        manager.SaveData();
    }
}
