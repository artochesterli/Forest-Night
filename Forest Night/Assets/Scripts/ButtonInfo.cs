using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public float AppearTime;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        GameObject Icon = transform.Find("Icon").gameObject;
        GameObject Text = transform.Find("Text").gameObject;

        Icon.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        Text.GetComponent<Text>().color = new Color(1, 1, 1, 0);

        float TimeCount = 0;
        while (TimeCount < AppearTime)
        {
            TimeCount += Time.deltaTime;
            Icon.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, TimeCount / AppearTime);
            Text.GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, TimeCount / AppearTime);
            yield return null;
        }
    }
}
