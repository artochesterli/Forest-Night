using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class QuitButton : MonoBehaviour
{
    public GameObject GameSceneMask;

    private const float FadeTime = 1;
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

    private IEnumerator QuitToMainPage()
    {
        float timecount = 0;
        while (timecount < FadeTime)
        {
            GameSceneMask.GetComponent<Image>().color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, timecount / FadeTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        GameSceneMask.GetComponent<Image>().color = Color.black;

        SceneManager.LoadScene("MainPage");
    }

    private void OnButtonClicked(ButtonClicked Click)
    {
        if (Click.Button == gameObject)
        {
            EventManager.instance.Fire(new GameSceneMenuClose());
            StartCoroutine(QuitToMainPage());
        }
    }
}
