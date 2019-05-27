using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuManager : MonoBehaviour
{
    public GameObject MainHelpMenu;

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
        if (Active)
        {
            if (InputClose())
            {
                EventManager.instance.Fire(new ExitMenu(gameObject));
                EventManager.instance.Fire(new EnterMenu(MainHelpMenu));
            }
        }
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = true;
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
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}
