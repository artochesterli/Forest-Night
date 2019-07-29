using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public float BlinkCycle;

    private bool Activated;

    private float TimeCount;
    private Color TextColor;

   

    // Start is called before the first frame update
    void Start()
    {
        Activated = true;
        TextColor = transform.GetChild(0).GetComponent<Text>().color;

        EventManager.instance.AddHandler<ExitMenu>(OnExitMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ExitMenu>(OnExitMenu);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        SetAppearance();
    }

    private void CheckInput()
    {
        if (InputAvaliable()&&Activated)
        {
            EventManager.instance.Fire(new EnterMenu(MainMenu));
            EventManager.instance.Fire(new ExitMenu(gameObject));
        }
    }

    private void SetAppearance()
    {
        if (Activated)
        {
            TimeCount += Time.deltaTime;
            if (TimeCount <= BlinkCycle / 2)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Text>() != null)
                    {
                        child.GetComponent<Text>().color = Color.Lerp(TextColor, new Color(TextColor.r, TextColor.g, TextColor.b, 0), TimeCount * 2 / BlinkCycle);
                    }

                    if (child.GetComponent<Image>() != null)
                    {
                        child.GetComponent<Image>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), TimeCount * 2 / BlinkCycle);
                    }
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Text>() != null)
                    {
                        child.GetComponent<Text>().color = Color.Lerp(new Color(TextColor.r, TextColor.g, TextColor.b, 0), TextColor,  (TimeCount-BlinkCycle/2) * 2 / BlinkCycle);
                    }

                    if (child.GetComponent<Image>() != null)
                    {
                        child.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, (TimeCount - BlinkCycle / 2) * 2 / BlinkCycle);
                    }
                }
                if (TimeCount >= BlinkCycle)
                {
                    TimeCount = 0;
                }
            }
        }
    }

    private bool InputAvaliable()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("A");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Return);
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            Activated = false;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
