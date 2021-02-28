using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interpolates between colours to make a rainbow effect
/// </summary>
public class ColourLerp : MonoBehaviour
{
    public Color[] colours;

    public int index = 0;
    private int nextIndex;

    public float lerpTime = 2f;

    private float lastChange = 0f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {

        if(colours == null || colours.Length < 2)
        {
            Debug.Log("No Colours to use!");
        }

        nextIndex = (index + 1) % colours.Length;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > lerpTime)
        {
            index = (index + 1) % colours.Length;
            nextIndex = (index + 1) % colours.Length;
            timer = 0f;
        }

        GetComponent<Image>().color = Color.Lerp(colours[index], colours[nextIndex], timer / lerpTime);
    }
}
