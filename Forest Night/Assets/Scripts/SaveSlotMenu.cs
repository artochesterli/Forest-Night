using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private bool Active;
    private int SelectedSlotIndex;

    private bool dataloaded;

    private const float StickYThreshold = 0.7f;
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
            SetFrame();
            SetInfo();
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (InputRight())
        {
            SelectedSlotIndex = (SelectedSlotIndex + 1) % Slots.Count;
        }

        if (InputLeft())
        {
            SelectedSlotIndex = SelectedSlotIndex - 1;
            if (SelectedSlotIndex < 0)
            {
                SelectedSlotIndex += Slots.Count;
            }
        }
    }

    private bool InputRight()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("RightArrow") || ControllerManager.MainCharacter.GetAxis("Left Stick X") > StickYThreshold;
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
            return ControllerManager.MainCharacter.GetButtonDown("LeftArrow") || ControllerManager.MainCharacter.GetAxis("Left Stick X") < -StickYThreshold;
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
            if (SaveDataManager.data.Progress[i] == 0)
            {
                Slots[i].transform.Find("Number").GetComponent<Text>().text = "";
            }
            else
            {
                Slots[i].transform.Find("Number").GetComponent<Text>().text = SaveDataManager.data.Progress[i].ToString();
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
        if (SaveDataManager.data.Progress[SelectedSlotIndex] == 0)
        {
            NewGameInfo.SetActive(true);
            LoadInfo.SetActive(false);
            DeleteInfo.SetActive(false);
        }
        else
        {
            NewGameInfo.SetActive(false);
            LoadInfo.SetActive(true);
            DeleteInfo.SetActive(true);
        }
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
            SetText();
            SetSlot();
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
            BackInfo.SetActive(false);
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].SetActive(false);
            }
            
        }
    }

}
