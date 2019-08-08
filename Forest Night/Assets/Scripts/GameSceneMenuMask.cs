using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneMenuMask : MonoBehaviour
{
    private bool Fading;
    private float timecount;

    private const float ToMainPageTime = 1;
    private const float FadeTime=0.2f;
    private const float FadeAlpha = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.AddHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
        timecount = FadeTime;
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.RemoveHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
    }

    // Update is called once per frame
    void Update()
    {
        SetFade();
    }

    private void SetFade()
    {
        timecount += Time.deltaTime;
        if (Fading)
        {
            GetComponent<Image>().color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, FadeAlpha), timecount / FadeTime);
        }
        else
        {
            GetComponent<Image>().color = Color.Lerp(new Color(0, 0, 0, FadeAlpha), new Color(0, 0, 0, 0), timecount / FadeTime);
        }
    }

    private void OnGameSceneMenuOpen(GameSceneMenuOpen M)
    {
        if (!Fading)
        {
            timecount = 0;
            Fading = true;
        }
    }

    private void OnGameSceneMenuClose(GameSceneMenuClose M)
    {
        timecount = 0;
        Fading = false;
    }

}
