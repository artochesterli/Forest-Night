using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Rewired;

public class MainMenuManager : MonoBehaviour
{
    public GameObject ContinueButton;
    public GameObject StartButton;
    public GameObject HelpButton;
    public GameObject OptionButton;
    public GameObject ExitButton;

    private Player MainCharacterPlayer;
    private Player FairyPlayer;
    private Dictionary<int, GameObject> IndexToButton;
    private int SelectedMenu;

    private bool ContinueAvaliable;
    private bool Active;

    private const string FolderName = "PlayerData";
    private const string FileName = "PlayerData";
    private const string Extension = ".dat";
    private const float StartY = 0;
    private const float Interval = 100;

    // Start is called before the first frame update
    void Start()
    {
        MainCharacterPlayer = ReInput.players.GetPlayer(0);
        FairyPlayer = ReInput.players.GetPlayer(1);
        Active = true;
        
        EventManager.instance.AddHandler<EnterMainMenu>(OnEnterMainMenu);
        EventManager.instance.AddHandler<ExitMainMenu>(OnExitMainMenu);

        SetMenu();
        SelectedMenu = 0;
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterMainMenu>(OnEnterMainMenu);
        EventManager.instance.RemoveHandler<ExitMainMenu>(OnExitMainMenu);
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
        SetMenuState();
    }

    private void CheckInput()
    {
        if (Active)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GetComponents<AudioSource>()[1].Play();
                if (SelectedMenu - 1 < 0)
                {
                    SelectedMenu += IndexToButton.Count;
                }
                SelectedMenu = (SelectedMenu - 1) % IndexToButton.Count;
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GetComponents<AudioSource>()[1].Play();
                SelectedMenu = (SelectedMenu + 1) % IndexToButton.Count;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventManager.instance.Fire(new ButtonClicked(IndexToButton[SelectedMenu]));
                GetComponents<AudioSource>()[0].Play();
            }

        }
    }

    private void SetMenuState()
    {
        if (Active)
        {
            for (int i = 0; i < IndexToButton.Count; i++)
            {
                if (SelectedMenu == i)
                {
                    IndexToButton[i].GetComponent<ButtonAppearance>().state = ButtonStatus.Selected;
                }
                else
                {
                    IndexToButton[i].GetComponent<ButtonAppearance>().state = ButtonStatus.NotSelected;
                }
            }
        }
    }

    private bool HaveData()
    {
        string FolderPath = Path.Combine(Application.dataPath, FolderName);
        string DataPath = Path.Combine(FolderPath, FileName + Extension);
        return File.Exists(DataPath);
    }


    private void OnExitMainMenu(ExitMainMenu E)
    {
        Active = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnEnterMainMenu(EnterMainMenu E)
    {
        Active = true;
        SetMenu();
    }

    private void SetMenu()
    {
        if (IndexToButton != null)
        {
            IndexToButton.Clear();
        }
        IndexToButton = new Dictionary<int, GameObject>();
        if (HaveData())
        {
            IndexToButton.Add(0, ContinueButton);
            IndexToButton.Add(1, StartButton);
            IndexToButton.Add(2, HelpButton);
            IndexToButton.Add(3, OptionButton);
            IndexToButton.Add(4, ExitButton);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, StartY - i * Interval);
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            IndexToButton.Add(0, StartButton);
            IndexToButton.Add(1, HelpButton);
            IndexToButton.Add(2, OptionButton);
            IndexToButton.Add(3, ExitButton);
            ContinueButton.SetActive(false);
            for (int i = 0; i < IndexToButton.Count; i++)
            {
                IndexToButton[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, StartY - i * Interval);
                IndexToButton[i].SetActive(true);
            }
        }

        
    }

}
