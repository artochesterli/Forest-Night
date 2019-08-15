using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitYesButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<ButtonClicked>(OnButtonClicked);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ButtonClicked>(OnButtonClicked);
    }

    private void OnButtonClicked(ButtonClicked B)
    {
        if (B.Button == gameObject)
        {
            EventManager.instance.Fire(new QuitGame(false));
            AcrossSceneInfo.AcrossGameLevel = false;
            SceneManager.LoadScene("MainPage");
        }
    }
}
