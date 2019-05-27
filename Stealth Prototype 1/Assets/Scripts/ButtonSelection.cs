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

    private bool EnableThisFrame;

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

    private void OnEnable()
    {
        EnableThisFrame = true;
        StartCoroutine(EnableSelf());
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

    private bool InputSelect()
    {
        if (EnableThisFrame)
        {
            return false;
        }

        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("A");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Return);
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
                    GetComponents<AudioSource>()[1].Play();
                    SelectedMenu++;
                    if (MenuShowImage)
                    {
                        GetComponent<MenuMoveImage>().MoveImage(false, SelectedMenu);
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
                        GetComponents<AudioSource>()[1].Play();
                        SelectedMenu++;
                        if (MenuShowImage)
                        {
                            GetComponent<MenuMoveImage>().MoveImage(false, SelectedMenu);
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
                    GetComponents<AudioSource>()[1].Play();
                    SelectedMenu--;
                    if (MenuShowImage)
                    {
                        GetComponent<MenuMoveImage>().MoveImage(true, SelectedMenu);
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
                        GetComponents<AudioSource>()[1].Play();
                        SelectedMenu--;
                        if (MenuShowImage)
                        {
                            GetComponent<MenuMoveImage>().MoveImage(true, SelectedMenu);
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

        if (InputSelect())
        {
            if (ButtonClickable)
            {
                EventManager.instance.Fire(new ButtonClicked(ButtonList[SelectedMenu]));
                GetComponents<AudioSource>()[0].Play();
            }
        }

    }

    private void SetMenuState()
    {
        for (int i = 0; i < ButtonList.Count; i++)
        {
            if (SelectedMenu == i)
            {
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

    private IEnumerator EnableSelf()
    {
        yield return null;
        EnableThisFrame = false;
    }
}
