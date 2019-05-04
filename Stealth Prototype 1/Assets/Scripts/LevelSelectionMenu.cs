using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionMenu : MonoBehaviour
{
    private bool Active;
    private int CurrentLevel;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<EnterLevelSelection>(OnEnterLevelSelection);
        EventManager.instance.AddHandler<ExitLevelSelection>(OnExitLevelSelection);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterLevelSelection>(OnEnterLevelSelection);
        EventManager.instance.RemoveHandler<ExitLevelSelection>(OnExitLevelSelection);
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
                EventManager.instance.Fire(new ExitLevelSelection());
                EventManager.instance.Fire(new EnterMainMenu());
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                EventManager.instance.Fire(new ExitLevelSelection());
                EventManager.instance.Fire(new EnterMainMenu());
            }
        }
    }

    private void OnEnterLevelSelection(EnterLevelSelection E)
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

    private void OnExitLevelSelection(ExitLevelSelection E)
    {
        Active = false;
        GetComponent<ButtonSelection>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
