using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool AbleToInvisible;
    public bool invisible;
    public float invisible_alpha;

    private float LightTimeCount = 0;

    private const float LightIntensity = 0.5f;
    private const float LightFadeTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCapability();
        ChangeLight();
        CheckStatus();
    }

    private void CheckCapability()
    {
        if (CompareTag("Main_Character"))
        {
            if (GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Normal&&GetComponent<CharacterMove>().OnGround)
            {
                AbleToInvisible = true;
            }
            else
            {
                AbleToInvisible = false;
            }
        }
        else if(CompareTag("Fairy"))
        {
            if (GetComponent<Fairy_Status_Manager>().status == FairyStatus.Normal && GetComponent<CharacterMove>().OnGround)
            {
                AbleToInvisible = true;
            }
            else
            {
                AbleToInvisible = false;
            }
        }
    }
 
    private void ChangeLight()
    {
        GameObject Light2D = transform.Find("2D Light").gameObject;
        GameObject EnvironmentLight = transform.Find("LightToEnvironment").gameObject;
        if (invisible)
        {
            LightTimeCount -= Time.deltaTime;
            if (LightTimeCount < 0)
            {
                LightTimeCount = 0;
            }
            Light2D.GetComponent<Light2D>().Intensity = LightTimeCount / LightFadeTime * LightIntensity;
            foreach(Transform child in EnvironmentLight.transform)
            {
                child.GetComponent<Light>().intensity= LightTimeCount / LightFadeTime * LightIntensity;
            }
        }
        else
        {
            LightTimeCount += Time.deltaTime;
            if (LightTimeCount > LightFadeTime)
            {
                LightTimeCount = LightFadeTime;
            }
            Light2D.GetComponent<Light2D>().Intensity = LightTimeCount / LightFadeTime * LightIntensity;
            foreach (Transform child in EnvironmentLight.transform)
            {
                child.GetComponent<Light>().intensity = LightTimeCount / LightFadeTime * LightIntensity;
            }

        }
    }

    private void CheckStatus()
    {
        if (invisible)
        {
            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(current_color.r, current_color.g, current_color.b, invisible_alpha);
            gameObject.layer = LayerMask.NameToLayer("Invisible_Object");
        }
        else
        {
            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(current_color.r, current_color.g, current_color.b, 1);
            if (CompareTag("Main_Character"))
            {
                gameObject.layer = LayerMask.NameToLayer("Main_Character");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Fairy");
            }
        }
    }
    
}
