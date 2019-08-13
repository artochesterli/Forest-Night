using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour
{
    public GameObject BackInfo;

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

    private void OnEnterMenu(EnterMenu E)
    {
        if (E.Menu == gameObject)
        {
            BackInfo.SetActive(true);
            MenuGroupManager.CurrentActivatedMenu = gameObject;
            transform.Find("InfoText").gameObject.SetActive(true);
            GetComponent<ButtonSelection>().enabled = true;
            for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
            {
                GetComponent<ButtonSelection>().ButtonList[i].SetActive(true);
            }
        }
    }

    private void OnExitMenu(ExitMenu E)
    {
        if (E.Menu == gameObject)
        {
            BackInfo.SetActive(false);
            transform.Find("InfoText").gameObject.SetActive(false);
            GetComponent<ButtonSelection>().enabled = false;
            GetComponent<ButtonSelection>().SelectedMenu = 0;
            for (int i = 0; i < GetComponent<ButtonSelection>().ButtonList.Count; i++)
            {
                GetComponent<ButtonSelection>().ButtonList[i].SetActive(false);
            }

        }
    }
}
