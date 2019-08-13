using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class TutorialFrame : MonoBehaviour
{

    private bool MenuOpening;
    private bool Opening;

    public GameObject Image;
    public GameObject Mask;
    public GameObject ConfirmInfo;


    private bool MenuCloseThisFrame;

    private const float OpenCloseTime = 0.3f;
    private const float MaskAlpha = 0.8f;

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
        if (Opening&&!MenuOpening&&!MenuCloseThisFrame)
        {
            if (InputAvailable())
            {
                EventManager.instance.Fire(new TutorialClose(gameObject));
                Opening = false;
            }
        }

        MenuCloseThisFrame = false;
    }

    private bool InputAvailable()
    {
        return ControllerManager.MainCharacter.GetButtonDown("A") || ControllerManager.Fairy.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Return);
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

        ConfirmInfo.SetActive(false);
    }

    private void OnGameSceneMenuClose(GameSceneMenuClose G)
    {
        MenuOpening = false;
        MenuCloseThisFrame = true;

        if (Opening)
        {
            ConfirmInfo.SetActive(true);
        }
    }


    private IEnumerator Open()
    {
        Image.SetActive(true);
        Image.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        Mask.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        ConfirmInfo.SetActive(true);

        float timecount = 0;
        while (timecount < OpenCloseTime)
        {
            Image.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timecount / OpenCloseTime);
            Mask.GetComponent<Image>().color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, MaskAlpha), timecount / OpenCloseTime);
            timecount += Time.deltaTime;
            yield return null;
        }

        Opening = true;
    }

    private IEnumerator Close()
    {
        Opening = false;

        Image.GetComponent<Image>().color = Color.white;

        Mask.GetComponent<Image>().color = new Color(0, 0, 0, MaskAlpha);

        ConfirmInfo.SetActive(false);

        float timecount = 0;
        while (timecount < OpenCloseTime)
        {
            Image.GetComponent<Image>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0),  timecount / OpenCloseTime);
            Mask.GetComponent<Image>().color = Color.Lerp(new Color(0, 0, 0, MaskAlpha), new Color(0, 0, 0, 0),  timecount / OpenCloseTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        Image.SetActive(false);

    }
}
