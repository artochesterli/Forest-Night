using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class AbilitiesMenuManager : MonoBehaviour
{
    private Player MainCharacterPlayer;
    private Player FairyPlayer;

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


    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            MenuGroupManager.CurrentActivatedMenu = gameObject;
            GetComponent<ButtonSelection>().enabled = true;
            GetComponent<MenuMoveImage>().enabled = true;
            var MenuMoveImage = GetComponent<MenuMoveImage>();
            MenuMoveImage.Image.GetComponent<Image>().enabled = true;
            MenuMoveImage.ImageIndex = GetComponent<ButtonSelection>().SelectedMenu;
            MenuMoveImage.SetSprite();
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
            GetComponent<ButtonSelection>().enabled = false;
            GetComponent<MenuMoveImage>().enabled = false;
            var MenuMoveImage = GetComponent<MenuMoveImage>();
            MenuMoveImage.Image.GetComponent<Image>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}
