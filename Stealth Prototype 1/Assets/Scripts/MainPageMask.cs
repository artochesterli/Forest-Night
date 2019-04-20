using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageMask : MonoBehaviour
{
    public Color MainMenuColor;
    public Color SubMenuColor;

    public float ChangeTime;

    private bool ToSubMenu;
    private float TimeCount;
    // Start is called before the first frame update
    void Start()
    {
        ToSubMenu = false;
        EventManager.instance.AddHandler<EnterMainMenu>(OnEnterMainMenu);
        EventManager.instance.AddHandler<ExitMainMenu>(OnExitMainMenu);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterMainMenu>(OnEnterMainMenu);
        EventManager.instance.RemoveHandler<ExitMainMenu>(OnExitMainMenu);
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

    private void OnEnterMainMenu(EnterMainMenu E)
    {
        ToSubMenu = false;
    }

    private void OnExitMainMenu(ExitMainMenu E)
    {
        ToSubMenu = true;
    }
}
