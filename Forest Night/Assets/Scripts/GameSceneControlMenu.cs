using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneControlMenu : MonoBehaviour
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

    private void OnEnterMenu(EnterMenu M)
    {
        if (M.Menu == gameObject)
        {
            BackInfo.SetActive(true);
            MenuGroupManager.CurrentActivatedMenu = gameObject;
            MenuGroupManager.CurrentSelectedButton = gameObject;
            Image.GetComponent<Image>().enabled = true;
            SetImage();
        }
    }

    private void OnExitMenu(ExitMenu M)
    {
        if(M.Menu == gameObject)
        {
            BackInfo.SetActive(false);
            GameObject Image = transform.Find("Image").gameObject;
            Image.GetComponent<Image>().enabled = false;
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
