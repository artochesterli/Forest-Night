using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHelpMenuManager : MonoBehaviour
{
    public GameObject AbilitiesButton;
    public GameObject ObjectsButton;
    public GameObject ControlButton;
    
    private Dictionary<int, GameObject> IndexToButton;
    private int SelectedMenu;

    private bool Active;
    private bool EnterThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        IndexToButton = new Dictionary<int, GameObject>();
        IndexToButton.Add(0, AbilitiesButton);
        IndexToButton.Add(1, ObjectsButton);
        IndexToButton.Add(2, ControlButton);
        SelectedMenu = 0;

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
        SetMenuState();
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

    private void CheckInput()
    {
        if (Active&&!EnterThisFrame)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (SelectedMenu - 1 < 0)
                {
                    SelectedMenu += IndexToButton.Count;
                }
                SelectedMenu = (SelectedMenu - 1) % IndexToButton.Count;
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectedMenu = (SelectedMenu + 1) % IndexToButton.Count;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventManager.instance.Fire(new ButtonClicked(IndexToButton[SelectedMenu]));
                return;
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
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnEnterMainHelpMenu(EnterMainHelpMenu E)
    {
        EnterThisFrame = true;
        Active = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
