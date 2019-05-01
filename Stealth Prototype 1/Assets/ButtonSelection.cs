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

    private Player MainCharacterPlayer;
    private Player FairyPlayer;

    // Start is called before the first frame update
    void Start()
    {
        MainCharacterPlayer = ReInput.players.GetPlayer(0);
        FairyPlayer = ReInput.players.GetPlayer(1);
        SelectedMenu = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        SetMenuState();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
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
}
