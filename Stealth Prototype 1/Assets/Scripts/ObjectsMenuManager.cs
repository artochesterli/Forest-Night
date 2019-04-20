using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsMenuManager : MonoBehaviour
{
    public GameObject VinesButton;
    public GameObject PlatformButton;
    public GameObject MirrorButton;

    private Dictionary<int, GameObject> IndexToButton;
    private int SelectedMenu;

    private bool Active;
    // Start is called before the first frame update
    void Start()
    {
        IndexToButton = new Dictionary<int, GameObject>();
        IndexToButton.Add(0, VinesButton);
        IndexToButton.Add(1, PlatformButton);
        IndexToButton.Add(2, MirrorButton);
        SelectedMenu = 0;

        EventManager.instance.AddHandler<EnterObjectsMenu>(OnEnterObjectsMenu);
        EventManager.instance.AddHandler<ExitObjectsMenu>(OnExitObjectsMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterObjectsMenu>(OnEnterObjectsMenu);
        EventManager.instance.RemoveHandler<ExitObjectsMenu>(OnExitObjectsMenu);
    }
    // Update is called once per frame
    void Update()
    {
        SetMenuState();
        CheckInput();
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

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                EventManager.instance.Fire(new EnterMainHelpMenu());
                EventManager.instance.Fire(new ExitObjectsMenu());
            }
        }
    }

    private void OnEnterObjectsMenu(EnterObjectsMenu E)
    {
        Active = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void OnExitObjectsMenu(ExitObjectsMenu E)
    {
        Active = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
