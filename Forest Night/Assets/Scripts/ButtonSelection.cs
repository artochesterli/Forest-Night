using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ButtonSelection : MonoBehaviour
{
    public bool ButtonClickable;
    public bool MenuShowImage;
    public List<GameObject> ButtonList;
    public int SelectedMenu;

    private float MoveTimeCount;
    private bool FirstPush;
    private bool Charging;
    private float ChargeTimeCount;

    private const float ChargeTime = 0.5f;
    private const float MoveTime = 0.1f;
    private const float StickYThreshold=0.7f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        SetMenuState();
    }

    private bool InputUp()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetAxis("Left Stick Y") > StickYThreshold || ControllerManager.MainCharacter.GetButton("UpArrow");
        }
        else
        {
            return Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
        }
    }

    private bool InputDown()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetAxis("Left Stick Y") < -StickYThreshold || ControllerManager.MainCharacter.GetButton("DownArrow");
        }
        else
        {
            return Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow);
        }
    }

    private void CheckInput()
    {
        if(InputDown())
        {
            if(SelectedMenu < ButtonList.Count - 1)
            {
                if (FirstPush)
                {
                    FirstPush = false;
                    GetComponent<AudioSource>().Play();
                    SelectedMenu++;
                    if (MenuShowImage)
                    {
                        GetComponent<MenuMoveImage>().MoveImage(SelectedMenu);
                    }

                }

                if (Charging)
                {
                    if (ChargeTimeCount > ChargeTime)
                    {
                        Charging = false;
                    }
                    ChargeTimeCount += Time.deltaTime;
                }
                else
                {
                    if (MoveTimeCount >= MoveTime)
                    {
                        GetComponent<AudioSource>().Play();
                        SelectedMenu++;
                        if (MenuShowImage)
                        {
                            GetComponent<MenuMoveImage>().MoveImage(SelectedMenu);
                        }
                        MoveTimeCount = 0;
                    }
                    MoveTimeCount += Time.deltaTime;
                }
            }
            else
            {
                ResetInputState();
            }
            
            
        }

        if (InputUp())
        {
            if(SelectedMenu >= 1)
            {
                if (FirstPush)
                {
                    FirstPush = false;
                    GetComponent<AudioSource>().Play();
                    SelectedMenu--;
                    if (MenuShowImage)
                    {
                        GetComponent<MenuMoveImage>().MoveImage(SelectedMenu);
                    }
                }

                if (Charging)
                {
                    if (ChargeTimeCount > ChargeTime)
                    {
                        Charging = false;
                    }
                    ChargeTimeCount += Time.deltaTime;
                }
                else
                {
                    if (MoveTimeCount >= MoveTime)
                    {
                        GetComponent<AudioSource>().Play();
                        SelectedMenu--;
                        if (MenuShowImage)
                        {
                            GetComponent<MenuMoveImage>().MoveImage(SelectedMenu);
                        }
                        MoveTimeCount = 0;
                    }
                    MoveTimeCount += Time.deltaTime;
                }
            }
            else
            {
                ResetInputState();
            }
            
        }

        if(!InputDown()&&!InputUp())
        {
            ResetInputState();
        }

    }

    private void SetMenuState()
    {
        for (int i = 0; i < ButtonList.Count; i++)
        {
            if (SelectedMenu == i)
            {
                MenuGroupManager.CurrentSelectedButton = ButtonList[i];
                ButtonList[i].GetComponent<ButtonAppearance>().state = ButtonStatus.Selected;
            }
            else
            {
                ButtonList[i].GetComponent<ButtonAppearance>().state = ButtonStatus.NotSelected;
            }
        }
    }

    private void ResetInputState()
    {
        FirstPush = true;
        ChargeTimeCount = 0;
        Charging = true;
        MoveTimeCount = MoveTime;
    }
}
