using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameSceneMenuManager : MonoBehaviour
{

    private bool Active;

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

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (ControllerManager.MainCharacter.GetButtonDown("Start"))
        {
            EventManager.instance.Fire(new EnterMenu(gameObject));
        }

        if (ControllerManager.MainCharacter.GetButtonDown("B"))
        {
            EventManager.instance.Fire(new ExitMenu(gameObject));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.instance.Fire(new EnterMenu(gameObject));
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            EventManager.instance.Fire(new ExitMenu(gameObject));
        }
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = true;
            GetComponent<ButtonSelection>().enabled = true;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = false;
            GetComponent<ButtonSelection>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
