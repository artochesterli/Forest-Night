using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public GameObject Mask;
    public int ConnectedLevel;
    public float ActivateDis;
    public Color ActivateColor;

    
    private bool Activated;

    private const float LightingTime = 1;
    private const float ScreenFadeTime = 1f;
    private const float ColorChangeTime = 0.2f;
    private const float PauseTime = 0.5f;
    private const int MaxLevel = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (AcrossSceneInfo.AcrossGameLevel)
        {
            StartCoroutine(ScreenAppearFade(Color.white,1,0,ScreenFadeTime));
        }
        else
        {
            StartCoroutine(ScreenAppearFade(Color.black,1,0,ScreenFadeTime));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Activated)
        {
            CheckCharacter();
        }
    }

    private void CheckCharacter()
    {
        if (Character_Manager.Main_Character != null && Character_Manager.Fairy != null)
        {
            if((transform.position-Character_Manager.Main_Character.transform.position).magnitude<ActivateDis && (transform.position - Character_Manager.Fairy.transform.position).magnitude < ActivateDis)
            {
                Activated = true;
                StartCoroutine(Activate());
            }
        }

    }

    private IEnumerator ChangeColor()
    {
        GameObject Gate = transform.Find("GoalPortal").gameObject;
        float timecount = 0;
        while (timecount < ColorChangeTime)
        {
            Gate.GetComponent<ParticleSystem>().startColor = Color.Lerp(Color.white, ActivateColor, timecount / ColorChangeTime);
            timecount += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Activate()
    {
        yield return StartCoroutine(ChangeColor());
        yield return new WaitForSeconds(PauseTime);
        GameObject Light = (GameObject)Instantiate(Resources.Load("Prefabs/GameObject/Light"));
        Light.GetComponent<Light>().intensity = 0;

        float timecount = 0;
        while (timecount < LightingTime)
        {
            Light.GetComponent<Light>().intensity = timecount / LightingTime;
            timecount += Time.deltaTime;
            yield return null;
        }

        timecount = 0;
        while (timecount < ScreenFadeTime)
        {
            Mask.GetComponent<Image>().color = Color.Lerp(new Color(1,1,1,0),Color.white, timecount / ScreenFadeTime);
            timecount += Time.deltaTime;
            yield return null;
        }
        if (ConnectedLevel <= MaxLevel)
        {
            AcrossSceneInfo.AcrossGameLevel = true;
            SceneManager.LoadSceneAsync("Level " + ConnectedLevel.ToString());
        }
        else
        {
            AcrossSceneInfo.AcrossGameLevel = false;
            EventManager.instance.Fire(new QuitGame(true));
            SceneManager.LoadSceneAsync("GameFinishScene");
        }
    }

    private IEnumerator ScreenAppearFade(Color C, float StartAlpha,float EndAlpha, float time)
    {
        float timecount = 0;
        while (timecount < time)
        {
            Mask.GetComponent<Image>().color = Color.Lerp(new Color(C.r, C.g, C.b, StartAlpha), new Color(C.r, C.g, C.b, EndAlpha), timecount / time);
            timecount += Time.deltaTime;
            yield return null;
        }
    }

}
