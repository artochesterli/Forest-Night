using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<EnterMainMenu>(OnEnterMainMenu);
        EventManager.instance.AddHandler<ExitMainMenu>(OnExitMainMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterMainMenu>(OnEnterMainMenu);
        EventManager.instance.RemoveHandler<ExitMainMenu>(OnExitMainMenu);
    }

    private void OnEnterMainMenu(EnterMainMenu E)
    {
        GetComponent<Image>().enabled = true;
    }

    private void OnExitMainMenu(ExitMainMenu E)
    {
        GetComponent<Image>().enabled = false;
    }
}
