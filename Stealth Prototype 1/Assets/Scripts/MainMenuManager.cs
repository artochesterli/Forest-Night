using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject ContinueButton;
    public GameObject StartButton;
    public GameObject TutorialButton;
    public GameObject OptionButton;
    public GameObject ExitButton;

    private Dictionary<int, GameObject> IndexToButton;
    private int SelectedMenu;

    private bool Active;

    // Start is called before the first frame update
    void Start()
    {
        Active = true;
        IndexToButton = new Dictionary<int, GameObject>();
        IndexToButton.Add(0, ContinueButton);
        IndexToButton.Add(1, StartButton);
        IndexToButton.Add(2, TutorialButton);
        IndexToButton.Add(3, OptionButton);
        IndexToButton.Add(4, ExitButton);

        SelectedMenu = 0;

        EventManager.instance.AddHandler<EnterMainMenu>(OnEnterMainMenu);
        EventManager.instance.AddHandler<ExitMainMenu>(OnExitMainMenu);

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
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

}
