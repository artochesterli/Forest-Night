using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.AddHandler<ExitMenu>(OnExitMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.RemoveHandler<ExitMenu>(OnExitMenu);
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == MainMenu)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == MainMenu)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
