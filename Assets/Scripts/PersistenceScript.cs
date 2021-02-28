using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceScript : MonoBehaviour
{
    public static PersistenceScript instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
