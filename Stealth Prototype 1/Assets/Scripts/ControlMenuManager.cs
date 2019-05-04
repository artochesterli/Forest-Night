using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuManager : MonoBehaviour
{
    private bool Active;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<EnterControlMenu>(OnEnterControlMenu);
        EventManager.instance.AddHandler<ExitControlMenu>(OnExitControlMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterControlMenu>(OnEnterControlMenu);
        EventManager.instance.RemoveHandler<ExitControlMenu>(OnExitControlMenu);
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Active)
        {
            if (MainPageControllerManager.MainCharacter.GetButtonDown("B"))
            {
                EventManager.instance.Fire(new ExitControlMenu());
                EventManager.instance.Fire(new EnterMainHelpMenu());
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                EventManager.instance.Fire(new ExitControlMenu());
                EventManager.instance.Fire(new EnterMainHelpMenu());
            }
        }
    }

    private void OnEnterControlMenu(EnterControlMenu E)
    {
        Active = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void OnExitControlMenu(ExitControlMenu E)
    {
        Active = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
