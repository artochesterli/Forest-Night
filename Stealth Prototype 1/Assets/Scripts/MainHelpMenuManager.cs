using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainHelpMenuManager : MonoBehaviour
{
    public GameObject MainMenu;

    private Player MainCharacterPlayer;
    private Player FairyPlayer;

    private bool Active;
    private bool EnterThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        MainCharacterPlayer = ReInput.players.GetPlayer(0);
        FairyPlayer = ReInput.players.GetPlayer(1);

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
        if (Active&&!EnterThisFrame)
        {
            if (InputClose())
            {
                EventManager.instance.Fire(new EnterMenu(MainMenu));
                EventManager.instance.Fire(new ExitMenu(gameObject));
            }
        }
        EnterThisFrame = false;
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            EnterThisFrame = true;
            Active = true;
            GetComponent<ButtonSelection>().enabled = true;
            foreach (Transform child in transform)
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
