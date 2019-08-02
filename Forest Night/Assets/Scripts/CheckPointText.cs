using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointText : MonoBehaviour
{
    public float FadeTime;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<SaveLevel>(OnSaveLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<SaveLevel>(OnSaveLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSaveLevel(SaveLevel S)
    {
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        float TimeCount = 0;
        while (TimeCount < FadeTime)
        {
            GetComponent<Text>().color = new Color(1, 1, 1, TimeCount / FadeTime);
            TimeCount += Time.deltaTime;
            yield return null;
        }
        GetComponent<Text>().color = Color.white;

        TimeCount = 0;
        while (TimeCount < FadeTime)
        {
            GetComponent<Text>().color = new Color(1, 1, 1, 1 - TimeCount / FadeTime);
            TimeCount += Time.deltaTime;
            yield return null;
        }
        GetComponent<Text>().color = new Color(1, 1, 1, 0);

    }
}
