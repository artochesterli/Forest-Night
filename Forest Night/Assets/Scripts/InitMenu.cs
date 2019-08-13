using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitMenu : MonoBehaviour
{
    public Color TextColor;
    public float BlinkCycle;
    public float BlinkAlpha;
    public GameObject ControllerIcon;
    public GameObject KeyboardIcon;

    private bool Activated;

    private float TimeCount;
    

    // Start is called before the first frame update
    void Start()
    {
        Activated = true;

        EventManager.instance.AddHandler<ExitMenu>(OnExitMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ExitMenu>(OnExitMenu);
    }

    // Update is called once per frame
    void Update()
    {
        SetAppearance();
    }

    private void SetAppearance()
    {
        if (Activated)
        {
            if (ControllerManager.MainCharacterJoystick != null)
            {
                ControllerIcon.SetActive(true);
                KeyboardIcon.SetActive(false);
            }
            else
            {
                ControllerIcon.SetActive(false);
                KeyboardIcon.SetActive(true);
            }

            TimeCount += Time.deltaTime;
            if (TimeCount <= BlinkCycle / 2)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Text>() != null)
                    {
                        child.GetComponent<Text>().color = Color.Lerp(TextColor, new Color(TextColor.r, TextColor.g, TextColor.b, BlinkAlpha), TimeCount * 2 / BlinkCycle);
                    }

                }
                ControllerIcon.GetComponent<Image>().color= Color.Lerp(Color.white, new Color(1, 1, 1, BlinkAlpha), TimeCount * 2 / BlinkCycle);
                KeyboardIcon.GetComponent<Image>().color = Color.Lerp(Color.white, new Color(1, 1, 1, BlinkAlpha), TimeCount * 2 / BlinkCycle);
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Text>() != null)
                    {
                        child.GetComponent<Text>().color = Color.Lerp(new Color(TextColor.r, TextColor.g, TextColor.b, BlinkAlpha), TextColor,  (TimeCount-BlinkCycle/2) * 2 / BlinkCycle);
                    }
                }

                ControllerIcon.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, BlinkAlpha), Color.white, (TimeCount - BlinkCycle / 2) * 2 / BlinkCycle);
                KeyboardIcon.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, BlinkAlpha), Color.white, (TimeCount - BlinkCycle / 2) * 2 / BlinkCycle);


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
