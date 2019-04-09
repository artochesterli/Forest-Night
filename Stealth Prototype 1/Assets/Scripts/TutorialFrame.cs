using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TutorialFrame : MonoBehaviour
{

    private Player MainCharacterPlayer;
    private Player FairyPlayer;

    private bool Opening;
    private const float OpenCloseTime = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        MainCharacterPlayer = ReInput.players.GetPlayer(0);
        FairyPlayer = ReInput.players.GetPlayer(1);

        EventManager.instance.AddHandler<TutorialOpen>(OnTutorialOpen);
        EventManager.instance.AddHandler<TutorialClose>(OnTutorialClose);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Opening)
        {
            if (MainCharacterPlayer.GetButtonDown("A") || FairyPlayer.GetButtonDown("A"))
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

    private IEnumerator Open()
    {
        GameObject Image = transform.Find("Image").gameObject;
        Image.transform.localScale = Vector3.zero;
        Image.GetComponent<SpriteRenderer>().enabled = true;
        float timecount = 0;
        while (timecount < OpenCloseTime)
        {
            Image.transform.localScale = Vector3.one * timecount / OpenCloseTime;
            timecount += Time.deltaTime;
            yield return null;
        }
        Image.transform.localScale = Vector3.one;
        Opening = true;
    }

    private IEnumerator Close()
    {
        Opening = false;
        GameObject Image = transform.Find("Image").gameObject;
        Image.transform.localScale = Vector3.one;
        
        float timecount = 0;
        while (timecount < OpenCloseTime)
        {
            Image.transform.localScale = Vector3.one * (1-timecount / OpenCloseTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        Image.GetComponent<SpriteRenderer>().enabled = false;
        Image.transform.localScale = Vector3.zero;
    }
}
