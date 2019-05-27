using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameSceneMenuManager : MonoBehaviour
{

    private bool Active;
    private bool EnterThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.AddHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
        EventManager.instance.AddHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.AddHandler<ExitMenu>(OnExitMenu);

    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.RemoveHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
        EventManager.instance.RemoveHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.RemoveHandler<ExitMenu>(OnExitMenu);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private bool InputOpen()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("Start");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }

    private bool InputClose()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("B");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }

    private void CheckInput()
    {
        if (!Active)
        {
            if (InputOpen())
            {
                EventManager.instance.Fire(new GameSceneMenuOpen());
            }
        }
        else if (!EnterThisFrame)
        {
            if (InputClose())
            {
                EventManager.instance.Fire(new GameSceneMenuClose());
            }
        }

        EnterThisFrame = false;
    }

    private void OnGameSceneMenuOpen(GameSceneMenuOpen M)
    {
        Active = true;
        GetComponent<ButtonSelection>().enabled = true;
        for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
        {
            GetComponent<ButtonSelection>().ButtonList[i].SetActive(true);
        }
    }

    private void OnGameSceneMenuClose(GameSceneMenuClose M)
    {
        Active = false;
        GetComponent<ButtonSelection>().enabled = false;
        for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
        {
            GetComponent<ButtonSelection>().ButtonList[i].SetActive(false);
        }
    }

    private void OnEnterMenu(EnterMenu M)
    {
        if (M.Menu == gameObject)
        {
            Active = true;
            EnterThisFrame = true;
            GetComponent<ButtonSelection>().enabled = true;
            for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
            {
                GetComponent<ButtonSelection>().ButtonList[i].SetActive(true);
            }
        }
    }

    private void OnExitMenu(ExitMenu M)
    {
        if (M.Menu == gameObject)
        {
            Active = false;
            GetComponent<ButtonSelection>().enabled = false;
            for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
            {
                GetComponent<ButtonSelection>().ButtonList[i].SetActive(false);
            }
        }
    }
}
