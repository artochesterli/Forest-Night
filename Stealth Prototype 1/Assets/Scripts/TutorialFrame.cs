using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TutorialFrame : MonoBehaviour
{

    private bool MenuOpening;
    private bool Opening;
    private const float OpenCloseTime = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<TutorialOpen>(OnTutorialOpen);
        EventManager.instance.AddHandler<TutorialClose>(OnTutorialClose);
        EventManager.instance.AddHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.AddHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<TutorialOpen>(OnTutorialOpen);
        EventManager.instance.RemoveHandler<TutorialClose>(OnTutorialClose);
        EventManager.instance.RemoveHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.RemoveHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Opening&&!MenuOpening)
        {
            if (ControllerManager.MainCharacter.GetButtonDown("A") || ControllerManager.Fairy.GetButtonDown("A"))
            {
                EventManager.instance.Fire(new TutorialClose(gameObject));
                Opening = false;
            }
        }
    }

    private void OnTutorialOpen(TutorialOpen T)
    {
        if (T.Tutorial == gameObject)
        {
            StartCoroutine(Open());
        }
    }

    private void OnTutorialClose(TutorialClose T)
    {
        if (T.Tutorial == gameObject)
        {
            StartCoroutine(Close());
        }
    }

    private void OnGameSceneMenuOpen(GameSceneMenuOpen G)
    {
        MenuOpening = true;
    }

    private void OnGameSceneMenuClose(GameSceneMenuClose G)
    {
        MenuOpening = false;
    }


    private IEnumerator Open()
    {
        GameObject Content = transform.Find("Content").gameObject;
        foreach(Transform child in Content.transform)
        {
            child.GetComponent<SpriteRenderer>().enabled = true;
            child.transform.localScale = Vector3.zero;
        }

        float timecount = 0;
        while (timecount < OpenCloseTime)
        {
            foreach (Transform child in Content.transform)
            {
                child.transform.localScale = Vector3.one * timecount / OpenCloseTime;
            }
            timecount += Time.deltaTime;
            yield return null;
        }

        Opening = true;
    }

    private IEnumerator Close()
    {
        Opening = false;
        GameObject Content = transform.Find("Content").gameObject;
        foreach (Transform child in Content.transform)
        {
            
            child.transform.localScale = Vector3.one;
        }

        float timecount = 0;
        while (timecount < OpenCloseTime)
        {
            foreach (Transform child in Content.transform)
            {
                child.transform.localScale = Vector3.one * (1-timecount / OpenCloseTime);
            }
            timecount += Time.deltaTime;
            yield return null;
        }
        foreach (Transform child in Content.transform)
        {
            child.GetComponent<SpriteRenderer>().enabled = false;
            child.transform.localScale = Vector3.zero;
        }

    }
}
