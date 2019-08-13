using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveSlotMenu : MonoBehaviour
{
    public GameObject NewGameInfo;
    public GameObject LoadInfo;
    public GameObject DeleteInfo;
    public GameObject BackInfo;

    public List<GameObject> Slots;

    public Sprite HaveDataFrameSelected;
    public Sprite HaveDataFrameNotSelected;
    public Sprite NoDataFrameSelected;
    public Sprite NoDataFrameNotSelected;

    public Color TextSelectedColor;
    public Color TextUnSelectedColor;

    private bool Active;
    private int SelectedSlotIndex;

    private bool dataloaded;

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
        if (Active)
        {
            MenuGroupManager.CurrentSelectedButton = Slots[SelectedSlotIndex];
            

            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (InputRight())
        {
            SelectedSlotIndex = (SelectedSlotIndex + 1) % Slots.Count;
            SetFrame();
            SetText();
            SetInfo();
        }

        if (InputLeft())
        {
            SelectedSlotIndex = SelectedSlotIndex - 1;
            if (SelectedSlotIndex < 0)
            {
                SelectedSlotIndex += Slots.Count;
            }
            SetFrame();
            SetText();
            SetInfo();
        }
    }

    private bool InputRight()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("RightArrow");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.RightArrow);
        }
    }

    private bool InputLeft()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("LeftArrow");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.LeftArrow);
        }
    }

    private void SetText()
    {
        for(int i = 0; i < Slots.Count; i++)
        {
            GameObject Number = Slots[i].transform.Find("Number").gameObject;
            GameObject LevelText= Slots[i].transform.Find("LevelText").gameObject;
            GameObject Date= Slots[i].transform.Find("Date").gameObject;
            if (SaveDataManager.data.Progress[i] == 0)
            {
                Number.GetComponent<Text>().text = "";
                LevelText.GetComponent<Text>().text = "";
                Date.GetComponent<Text>().text = "";
            }
            else
            {
                Number.GetComponent<Text>().text = SaveDataManager.data.Progress[i].ToString();
                LevelText.GetComponent<Text>().text = "LEVEL";
                DateTime datetime = SaveDataManager.data.Date[i];
                Date.GetComponent<Text>().text = datetime.Month.ToString()+"/"+datetime.Day.ToString()+"/"+datetime.Year.ToString();
            }
            if (SelectedSlotIndex == i)
            {
                Number.GetComponent<Text>().color = TextSelectedColor;
                LevelText.GetComponent<Text>().color = TextSelectedColor;
                Date.GetComponent<Text>().color = TextSelectedColor;
            }
            else
            {
                Number.GetComponent<Text>().color = TextUnSelectedColor;
                LevelText.GetComponent<Text>().color = TextUnSelectedColor;
                Date.GetComponent<Text>().color = TextUnSelectedColor;
            }
        }
    }

    private void SetFrame()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            GameObject Frame = Slots[i].transform.Find("Frame").gameObject;
            if (SaveDataManager.data.Progress[i] == 0)
            {
                if (i == SelectedSlotIndex)
                {
                    Frame.GetComponent<Image>().sprite = NoDataFrameSelected;
                }
                else
                {
                    Frame.GetComponent<Image>().sprite = NoDataFrameNotSelected;
                }
                
            }
            else
            {
                if (i == SelectedSlotIndex)
                {
                    Frame.GetComponent<Image>().sprite = HaveDataFrameSelected;
                }
                else
                {
                    Frame.GetComponent<Image>().sprite = HaveDataFrameNotSelected;
                }
            }
        }
    }

    private void SetInfo()
    {
        BackInfo.SetActive(false);
        NewGameInfo.SetActive(false);
        LoadInfo.SetActive(false);
        DeleteInfo.SetActive(false);
        if (SaveDataManager.data.Progress[SelectedSlotIndex] == 0)
        {
            NewGameInfo.SetActive(true);
        }
        else
        {
            LoadInfo.SetActive(true);
            DeleteInfo.SetActive(true);
        }
        BackInfo.SetActive(true);
    }

    private void SetSlot()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].GetComponent<SaveSlot>().Level = SaveDataManager.data.Progress[i];
        }
    }

    

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = true;
            MenuGroupManager.CurrentActivatedMenu = gameObject;
            BackInfo.SetActive(true);
            for(int i = 0; i < Slots.Count; i++)
            {
                Slots[i].SetActive(true);
            }
            SetFrame();
            SetText();
            SetInfo();
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = false;
            BackInfo.SetActive(false);
            NewGameInfo.SetActive(false);
            LoadInfo.SetActive(false);
            DeleteInfo.SetActive(false);
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].SetActive(false);
            }
            
        }
    }

}
