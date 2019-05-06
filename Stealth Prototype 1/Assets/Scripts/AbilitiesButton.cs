using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesButton : MonoBehaviour
{
    public GameObject AbilitiesMenu;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<ButtonClicked>(OnButtonClicked);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ButtonClicked>(OnButtonClicked);
    }


    private void OnButtonClicked(ButtonClicked Click)
    {

        if (Click.Button == gameObject)
        {
            EventManager.instance.Fire(new ExitMenu(transform.parent.gameObject));
            EventManager.instance.Fire(new EnterMenu(AbilitiesMenu));
        }
    }
}
