using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageMask : MonoBehaviour
{
    public GameObject MainMenu;
    public Color MainMenuColor;
    public Color SubMenuColor;

    public float ChangeTime;

    private bool ToSubMenu;
    private float TimeCount;
    // Start is called before the first frame update
    void Start()
    {
        ToSubMenu = false;
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
        if (ToSubMenu)
        {
            TimeCount += Time.deltaTime;
            if (TimeCount > ChangeTime)
            {
                TimeCount = ChangeTime;
            }
        }
        else
        {
            TimeCount -= Time.deltaTime;
            if (TimeCount < 0)
            {
                TimeCount = 0;
            }
        }
        GetComponent<RawImage>().color = Color.Lerp(MainMenuColor, SubMenuColor, TimeCount/ChangeTime);
    }

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == MainMenu)
        {
            ToSubMenu = false;
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == MainMenu)
        {
            ToSubMenu = true;
        }
    }
}
