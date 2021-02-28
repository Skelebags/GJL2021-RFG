using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour
{
    private Navigator navigator;

    // Start is called before the first frame update
    void Awake()
    {
        navigator = GameObject.Find("UI").GetComponent<Navigator>();
    }

    public void GoBack()
    {
        Destroy(gameObject);
    }

    public void QuitGame()
    {
        navigator.ToStartMenu();
    }
}
