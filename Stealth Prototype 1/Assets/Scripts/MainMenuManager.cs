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

    private bool Clicking;
    // Start is called before the first frame update
    void Start()
    {
        IndexToButton = new Dictionary<int, GameObject>();
        IndexToButton.Add(0, ContinueButton);
        IndexToButton.Add(1, StartButton);
        IndexToButton.Add(2, TutorialButton);
        IndexToButton.Add(3, OptionButton);
        IndexToButton.Add(4, ExitButton);

        SelectedMenu = 0;

        EventManager.instance.AddHandler<FinishClick>(OnFinishClick);

    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<FinishClick>(OnFinishClick);
    }
    // Update is called once per frame
    void Update()
    {
        SwtichMenu();
        SetMenuState();
    }

    private void SwtichMenu()
    {
        if (!Clicking)
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
                var Button = IndexToButton[SelectedMenu].GetComponent<ButtonAppearance>();
                Button.StartCoroutine(Button.Clicking());
                Clicking = true;
            }
        }
    }

    private void SetMenuState()
    {
        if (!Clicking)
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

    private void OnFinishClick(FinishClick F)
    {
        if (F.type == ButtonType.MainMenu)
        {
            Clicking = false;
        }
    }


}
