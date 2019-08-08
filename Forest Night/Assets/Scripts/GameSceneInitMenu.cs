using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneInitMenu : MonoBehaviour
{
    private bool Active;
    // Start is called before the first frame update
    void Start()
    {
        Active = true;
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
        if (Active && InputAvailable())
        {
            EventManager.instance.Fire(new ExitMenu(gameObject));
            EventManager.instance.Fire(new GameSceneMenuOpen());
        }
    }

    private bool InputAvailable()
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

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = true;
            EventManager.instance.Fire(new GameSceneMenuClose());
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = false;
        }
    }
}
