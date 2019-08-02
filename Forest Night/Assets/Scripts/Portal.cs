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

    // Start is called before the first frame update
    void Start()
    {
        GameObject SceneLoadData = GameObject.Find("SceneLoadData");
        if (SceneLoadData!=null&&SceneLoadData.GetComponent<SceneLoadData>().FromOtherLevel)
        {
            StartCoroutine(ScreenFade(Color.white));
        }
        else
        {
            StartCoroutine(ScreenFade(Color.black));
        }
        Destroy(SceneLoadData);
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
            var main = Gate.GetComponent<ParticleSystem>();
            main.startColor = Color.Lerp(Color.white, ActivateColor, timecount / ColorChangeTime);
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
        if (ConnectedLevel > 0)
        {
            GameObject SceneLoadData = (GameObject)Instantiate(Resources.Load("Prefabs/GameObject/SceneLoadData"));
            SceneLoadData.GetComponent<SceneLoadData>().FromOtherLevel = true;
            SceneLoadData.name = "SceneLoadData";
            DontDestroyOnLoad(SceneLoadData);
            SceneManager.LoadSceneAsync("Level " + ConnectedLevel.ToString());
        }
        else
        {
            SceneManager.LoadSceneAsync("MainPage");
        }
    }

    private IEnumerator ScreenFade(Color C)
    {
        float timecount = 0;
        while (timecount < ScreenFadeTime)
        {
            Mask.GetComponent<Image>().color = Color.Lerp(C, new Color(C.r, C.g, C.b, 0), timecount / ScreenFadeTime);
            timecount += Time.deltaTime;
            yield return null;
        }
    }

}
