using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionMenu : MonoBehaviour
{
    public GameObject MainMenu;

    private bool Active;
    private int CurrentLevel;
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
        if (Active)
        {
            if (MainPageControllerManager.MainCharacter.GetButtonDown("B"))
            {
                EventManager.instance.Fire(new ExitMenu(gameObject));
                EventManager.instance.Fire(new EnterMenu(MainMenu));
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                EventManager.instance.Fire(new ExitMenu(gameObject));
                EventManager.instance.Fire(new EnterMenu(MainMenu));
            }
        }
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = true;
            GetComponent<ButtonSelection>().enabled = true;
            CurrentLevel = SaveDataManager.LoadData().CurrentLevel;
            GetComponent<ButtonSelection>().ButtonList.Clear();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<LevelButton>().Level <= CurrentLevel)
                {
                    child.gameObject.SetActive(true);
                    GetComponent<ButtonSelection>().ButtonList.Add(child.gameObject);
                }
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
