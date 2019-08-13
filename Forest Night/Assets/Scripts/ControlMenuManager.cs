using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMenuManager : MonoBehaviour
{
    public GameObject BackInfo;
    public GameObject Image;
    public Sprite ControllerSprite;
    public Sprite KeyboardSprite;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.AddHandler<ExitMenu>(OnExitMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterMenu>(OnEnterMenu);
        EventManager.instance.RemoveHandler<ExitMenu>(OnExitMenu);
    }

    private void Update()
    {
        SetImage();
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            MenuGroupManager.CurrentActivatedMenu = gameObject;
            BackInfo.SetActive(true);
            Image.SetActive(true);
            SetImage();
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            BackInfo.SetActive(false);
            Image.SetActive(false);
        }
    }

    private void SetImage()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            Image.GetComponent<Image>().sprite = ControllerSprite;
        }
        else
        {
            Image.GetComponent<Image>().sprite = KeyboardSprite;
        }
    }

}
