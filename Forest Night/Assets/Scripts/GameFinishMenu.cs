using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFinishMenu : MonoBehaviour
{
    public GameObject ConfirmInfo;
    public GameObject Text;
    public GameObject Mask;
    public Color TextColor;

    public float ScreenAppearTime;
    public float ScreenFadeTime;
    public float TextAppearWaitTime;
    public float TextAppearTime;
    public float ConfirmInfoAppearWaitTime;

    private bool Return;

    void Start()
    {
        StartCoroutine(Show());
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (InputAvailable()&&!Return)
        {
            Return = true;
            StartCoroutine(ReturnToMain());
        }
    }

    private bool InputAvailable()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("A");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Return);
        }
    }

    private IEnumerator ReturnToMain()
    {
        yield return StartCoroutine(ScreenAppearFade(Color.black, 0, 1,ScreenFadeTime));

        SceneManager.LoadSceneAsync("MainPage");
    }

    private IEnumerator Show()
    {
        yield return StartCoroutine(ScreenAppearFade(Color.white, 1, 0,ScreenAppearTime));

        yield return new WaitForSeconds(TextAppearWaitTime);

        float TimeCount = 0;
        while (TimeCount < TextAppearTime)
        {
            TimeCount += Time.deltaTime;
            Text.GetComponent<Text>().color = Color.Lerp(new Color(TextColor.r, TextColor.g, TextColor.b, 0), TextColor, TimeCount / TextAppearTime);
            yield return null;
        }

        yield return new WaitForSeconds(ConfirmInfoAppearWaitTime);

        ConfirmInfo.SetActive(true);
    }

    private IEnumerator ScreenAppearFade(Color color,float startalpha,float endalpha, float time)
    {
        float TimeCount = 0;

        Mask.GetComponent<Image>().color = new Color(color.r, color.g, color.b, startalpha);
        while (TimeCount < time)
        {
            TimeCount += Time.deltaTime;
            Mask.GetComponent<Image>().color = Color.Lerp(new Color(color.r, color.g, color.b, startalpha), new Color(color.r, color.g, color.b, endalpha), TimeCount / time);
            yield return null;
        }
    }

    




}
