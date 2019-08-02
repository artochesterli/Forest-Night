using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneControlMenu : MonoBehaviour
{

    private const int ArrowUnlockLevel = 3;
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

    private void OnEnterMenu(EnterMenu M)
    {
        if (M.Menu == gameObject)
        {
            MenuGroupManager.CurrentActivatedMenu = gameObject;
            MenuGroupManager.CurrentSelectedButton = null;
            GameObject Image = transform.Find("Image").gameObject;
            Image.GetComponent<Image>().enabled = true;
            if (Level_Manager.Self.GetComponent<Level_Manager>().LevelIndex >= ArrowUnlockLevel)
            {
                Image.GetComponent<Image>().sprite = Resources.Load("Sprite/TutorialImage/ControlAll", typeof(Sprite)) as Sprite;
            }
            else
            {
                Image.GetComponent<Image>().sprite = Resources.Load("Sprite/TutorialImage/ControlWithoutShooting", typeof(Sprite)) as Sprite;
            }

        }
    }

    private void OnExitMenu(ExitMenu M)
    {
        if(M.Menu == gameObject)
        {
            GameObject Image = transform.Find("Image").gameObject;
            Image.GetComponent<Image>().enabled = false;
        }
    }
}
