using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainHelpMenuManager : MonoBehaviour
{

    private Player MainCharacterPlayer;
    private Player FairyPlayer;

    private bool Active;
    private bool EnterThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        MainCharacterPlayer = ReInput.players.GetPlayer(0);
        FairyPlayer = ReInput.players.GetPlayer(1);

        EventManager.instance.AddHandler<EnterMainHelpMenu>(OnEnterMainHelpMenu);
        EventManager.instance.AddHandler<ExitMainHelpMenu>(OnExitMainHelpMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterMainHelpMenu>(OnEnterMainHelpMenu);
        EventManager.instance.RemoveHandler<ExitMainHelpMenu>(OnExitMainHelpMenu);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Active&&!EnterThisFrame)
        {
            if (MainPageControllerManager.MainCharacter.GetButtonDown("B"))
            {
                EventManager.instance.Fire(new EnterMainMenu());
                EventManager.instance.Fire(new ExitMainHelpMenu());
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                EventManager.instance.Fire(new EnterMainMenu());
                EventManager.instance.Fire(new ExitMainHelpMenu());
            }
        }
        EnterThisFrame = false;
    }

    private void OnExitMainHelpMenu(ExitMainHelpMenu E)
    {
        Active = false;
        GetComponent<ButtonSelection>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnEnterMainHelpMenu(EnterMainHelpMenu E)
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
