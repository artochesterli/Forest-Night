using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public float AppearTime;
    public GameObject ControllerIcon;
    public GameObject KeyboardIcon;

    private void OnEnable()
    {
        SetAppearance();
        StopAllCoroutines();
        StartCoroutine(Appear());
    }

    private void Update()
    {
        SetAppearance();
    }

    private void SetAppearance()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            ControllerIcon.SetActive(true);
            KeyboardIcon.SetActive(false);
        }
        else
        {
            ControllerIcon.SetActive(false);
            KeyboardIcon.SetActive(true);
        }
    }

    private IEnumerator Appear()
    {

        GameObject Text = transform.Find("Text").gameObject;

        ControllerIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        KeyboardIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        Text.GetComponent<Text>().color = new Color(1, 1, 1, 0);

        float TimeCount = 0;
        while (TimeCount < AppearTime)
        {
            TimeCount += Time.deltaTime;
            ControllerIcon.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, TimeCount / AppearTime);
            KeyboardIcon.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, TimeCount / AppearTime);
            Text.GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, TimeCount / AppearTime);
            yield return null;
        }
    }
}
