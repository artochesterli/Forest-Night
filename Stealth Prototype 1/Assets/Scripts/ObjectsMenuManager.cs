using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ObjectsMenuManager : MonoBehaviour
{
    public GameObject MainHelpMenu;

    private Player MainCharacterPlayer;
    private Player FairyPlayer;

    private bool Active;

    private const float height = 1080;
    // Start is called before the first frame update
    void Start()
    {
        MainCharacterPlayer = ReInput.players.GetPlayer(0);
        FairyPlayer = ReInput.players.GetPlayer(1);

        EventManager.instance.AddHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.AddHandler<ExitMenu>(OnExitMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.RemoveHandler<ExitMenu>(OnExitMenu);
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private bool InputClose()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("B");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }

    private void CheckInput()
    {
        if (Active)
        {
            if (InputClose())
            {
                EventManager.instance.Fire(new EnterMenu(MainHelpMenu));
                EventManager.instance.Fire(new ExitMenu(gameObject));
            }
        }
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = true;
            GetComponent<ButtonSelection>().enabled = true;
            GetComponent<MenuMoveImage>().enabled = true;
            var MenuMoveImage = GetComponent<MenuMoveImage>();
            MenuMoveImage.Image.GetComponent<Image>().enabled = true;
            MenuMoveImage.Image.GetComponent<Image>().sprite = GetComponent<MenuMoveImage>().SpriteList[GetComponent<ButtonSelection>().SelectedMenu];
            MenuMoveImage.BackImage.GetComponent<Image>().enabled = true;
            MenuMoveImage.BackImage.GetComponent<Image>().sprite = GetComponent<MenuMoveImage>().SpriteList[GetComponent<ButtonSelection>().SelectedMenu];
            MenuMoveImage.BackImage.GetComponent<RectTransform>().anchoredPosition = MenuMoveImage.Image.GetComponent<RectTransform>().anchoredPosition + Vector2.up * height;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            Active = false;
            GetComponent<ButtonSelection>().enabled = false;
            GetComponent<MenuMoveImage>().enabled = false;
            var MenuMoveImage = GetComponent<MenuMoveImage>();
            MenuMoveImage.Image.GetComponent<Image>().enabled = false;
            MenuMoveImage.BackImage.GetComponent<Image>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}
