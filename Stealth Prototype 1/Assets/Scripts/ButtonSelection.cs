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
    private bool First;
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

    private void CheckInput()
    {
        if(ControllerManager.MainCharacter.GetAxis("Left Stick Y")<-StickYThreshold)
        {
            if (First)
            {
                First = false;
                if (SelectedMenu < ButtonList.Count - 1)
                {
                    GetComponents<AudioSource>()[1].Play();
                    SelectedMenu++;
                    if (MenuShowImage)
                    {
                        GetComponent<MenuMoveImage>().MoveImage(false, SelectedMenu);
                    }
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
                    if (SelectedMenu < ButtonList.Count - 1)
                    {
                        GetComponents<AudioSource>()[1].Play();
                        SelectedMenu++;
                        if (MenuShowImage)
                        {
                            GetComponent<MenuMoveImage>().MoveImage(false, SelectedMenu);
                        }
                    }
                    MoveTimeCount = 0;
                }
                MoveTimeCount += Time.deltaTime;
            }
        }

        if (ControllerManager.MainCharacter.GetAxis("Left Stick Y") > StickYThreshold)
        {
            if (First)
            {
                First = false;
                if (SelectedMenu >= 1)
                {
                    GetComponents<AudioSource>()[1].Play();
                    SelectedMenu--;
                    if (MenuShowImage)
                    {
                        GetComponent<MenuMoveImage>().MoveImage(true, SelectedMenu);
                    }
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
                    if (SelectedMenu >= 1)
                    {
                        GetComponents<AudioSource>()[1].Play();
                        SelectedMenu--;
                        if (MenuShowImage)
                        {
                            GetComponent<MenuMoveImage>().MoveImage(true, SelectedMenu);
                        }
                    }
                    MoveTimeCount = 0;
                }
                MoveTimeCount += Time.deltaTime;
            }
        }

        if(Mathf.Abs(ControllerManager.MainCharacter.GetAxis("Left Stick Y")) < StickYThreshold)
        {
            First = true;
            ChargeTimeCount = 0;
            Charging = true;
            MoveTimeCount = MoveTime;
        }

        if (ControllerManager.MainCharacter.GetButtonDown("A"))
        {
            if (ButtonClickable)
            {
                EventManager.instance.Fire(new ButtonClicked(ButtonList[SelectedMenu]));
                GetComponents<AudioSource>()[0].Play();
            }
        }

        /*if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponents<AudioSource>()[1].Play();
            if (SelectedMenu - 1 < 0)
            {
                SelectedMenu += ButtonList.Count;
            }
            SelectedMenu = (SelectedMenu - 1) % ButtonList.Count;
            if (MenuShowImage)
            {
                GetComponent<MenuMoveImage>().MoveImage(true, SelectedMenu);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponents<AudioSource>()[1].Play();
            SelectedMenu = (SelectedMenu + 1) % ButtonList.Count;
            if (MenuShowImage)
            {
                GetComponent<MenuMoveImage>().MoveImage(false, SelectedMenu);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ButtonClickable)
            {
                EventManager.instance.Fire(new ButtonClicked(ButtonList[SelectedMenu]));
                GetComponents<AudioSource>()[0].Play();
            }
        }*/
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
}
