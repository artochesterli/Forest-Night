using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public float DisMax;
    public float DisMin;
    public Color ActivateColor;

    private bool activated;
    private const float ColorChangeTime = 0.2f;
    private const float FadeTime = 2f;
    private const float FadePauseTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }
    // Update is called once per frame
    void Update()
    {
        CheckStatus();
    }

    private void CheckStatus()
    {
        if (!activated)
        {
            float MainCharacterDis = DisMin;
            if ((transform.position - Character_Manager.Main_Character.transform.position).magnitude > MainCharacterDis)
            {
                MainCharacterDis = (transform.position - Character_Manager.Main_Character.transform.position).magnitude;
                if (MainCharacterDis > DisMax)
                {
                    MainCharacterDis = DisMax;
                }
            }
            float FairyDis = DisMin;
            if ((transform.position - Character_Manager.Fairy.transform.position).magnitude > FairyDis)
            {
                FairyDis = (transform.position - Character_Manager.Fairy.transform.position).magnitude;
                if (FairyDis > DisMax)
                {
                    FairyDis = DisMax;
                }
            }

            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (0.5f * (MainCharacterDis - DisMin) + 0.5f * (FairyDis - DisMin)) / (DisMax - DisMin));

            if (MainCharacterDis <= DisMin && FairyDis <= DisMin)
            {
                activated = true;
                StartCoroutine(ChangeColor());
                EventManager.instance.Fire(new SaveLevel());
            }
        }
    }

    private IEnumerator ChangeColor()
    {
        float timecount = 0;
        while (timecount < ColorChangeTime)
        {
            timecount += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, ActivateColor, timecount / ColorChangeTime);
            yield return null;
        }
        yield return StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        GetComponent<SpriteRenderer>().color = ActivateColor;

        yield return new WaitForSeconds(FadePauseTime);
        float timecount = 0;
        while (timecount < FadeTime)
        {
            timecount += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(ActivateColor, new Color(ActivateColor.r, ActivateColor.g, ActivateColor.b, 0), timecount / FadeTime);
            yield return null;
        }
    }

    private void OnLoadLevel(LoadLevel L)
    {
        if (activated)
        {
            StartCoroutine(Fade());
        }
    }
}
