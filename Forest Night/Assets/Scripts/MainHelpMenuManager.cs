using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainHelpMenuManager : MonoBehaviour
{
    public GameObject MainMenu;

    private Player MainCharacterPlayer;
    private Player FairyPlayer;

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

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            MenuGroupManager.CurrentActivatedMenu = gameObject;
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
            GetComponent<ButtonSelection>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
