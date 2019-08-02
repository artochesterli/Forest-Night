using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameSceneMenuManager : MonoBehaviour
{

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
        
    }

    private void CheckInput()
    {
        
    }

    private void OnGameSceneMenuOpen(GameSceneMenuOpen M)
    {
        MenuGroupManager.CurrentActivatedMenu = gameObject;
        GetComponent<ButtonSelection>().enabled = true;
        for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
        {
            GetComponent<ButtonSelection>().ButtonList[i].SetActive(true);
        }
    }

    private void OnGameSceneMenuClose(GameSceneMenuClose M)
    {
        MenuGroupManager.CurrentActivatedMenu = null;
        MenuGroupManager.CurrentSelectedButton = null;
        GetComponent<ButtonSelection>().enabled = false;
        for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
        {
            GetComponent<ButtonSelection>().ButtonList[i].SetActive(false);
        }
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            MenuGroupManager.CurrentActivatedMenu = gameObject;
            GetComponent<ButtonSelection>().enabled = true;
            for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
            {
                GetComponent<ButtonSelection>().ButtonList[i].SetActive(true);
            }
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            GetComponent<ButtonSelection>().enabled = false;
            for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
            {
                GetComponent<ButtonSelection>().ButtonList[i].SetActive(false);
            }
        }
    }
}
