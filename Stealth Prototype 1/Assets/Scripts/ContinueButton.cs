using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public GameObject LevelSelection;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<ButtonClicked>(OnButtonClicked);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ButtonClicked>(OnButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClicked(ButtonClicked Click)
    {
        if (Click.Button == gameObject)
        {
            EventManager.instance.Fire(new EnterMenu(LevelSelection));
            EventManager.instance.Fire(new ExitMenu(transform.parent.gameObject));
        }
    }

}
