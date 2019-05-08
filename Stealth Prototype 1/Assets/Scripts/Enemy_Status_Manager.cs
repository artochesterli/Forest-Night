using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyStatus
{
    Patrol,
    Alert,
    ShootCharacter,
    ShootStar,
    AlertRelease,
    Stunned
}

public class Enemy_Status_Manager : MonoBehaviour
{
    public EnemyStatus status;
    public GameObject Indicator;
    public GameObject IndicatorRed;
    public GameObject AngryEffect;


    // Start is called before the first frame update
    void Start()
    {
        AngryEffect.GetComponent<ParticleSystem>().Stop(true);
        DeActivateStunEffect();
        EventManager.instance.AddHandler<CharacterDied>(OnCharacterDied);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CharacterDied>(OnCharacterDied);
    }

    // Update is called once per frame
    void Update()
    {
        SetIndicatorIcon();
        set_status();
    }

    private void SetIndicatorIcon()
    {
        if (status == EnemyStatus.Patrol)
        {
            Indicator.GetComponent<Image>().enabled = false;
            IndicatorRed.GetComponent<Image>().enabled = false;
            AngryEffect.GetComponent<ParticleSystem>().Stop(true);
        }
        else if(status==EnemyStatus.Alert || status == EnemyStatus.AlertRelease)
        {
            var EnemyCheck = GetComponent<Enemy_Check>();
            Indicator.GetComponent<Image>().enabled = true;
            IndicatorRed.GetComponent<Image>().enabled = true;
            IndicatorRed.GetComponent<Image>().fillAmount = EnemyCheck.alert_time_count / EnemyCheck.Alert_Time;
            AngryEffect.GetComponent<ParticleSystem>().Stop(true);
        }
        else if (status == EnemyStatus.ShootCharacter)
        {
            var EnemyCheck = GetComponent<Enemy_Check>();
            Indicator.GetComponent<Image>().enabled = true;
            IndicatorRed.GetComponent<Image>().enabled = true;
            IndicatorRed.GetComponent<Image>().fillAmount = 1;
            if (!AngryEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                AngryEffect.GetComponent<ParticleSystem>().Play(true);
            }
        }
        else if (status == EnemyStatus.ShootStar)
        {
            Indicator.GetComponent<Image>().enabled = false;
            IndicatorRed.GetComponent<Image>().enabled = false;
            AngryEffect.GetComponent<ParticleSystem>().Stop(true);
        }
        else if (status == EnemyStatus.Stunned)
        {
            Indicator.GetComponent<Image>().enabled = false;
            IndicatorRed.GetComponent<Image>().enabled = false;
            AngryEffect.GetComponent<ParticleSystem>().Stop(true);
        }
    }


    private void set_status()
    {
        GameObject view = transform.Find("View").gameObject;
        if (status == EnemyStatus.ShootCharacter || status==EnemyStatus.Stunned)
        {
            view.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            view.GetComponent<MeshRenderer>().enabled = true;
            if (transform.right.x > 0)
            {
                view.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                view.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    private void OnCharacterDied(CharacterDied C)
    {
        status = EnemyStatus.Patrol;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;

        if (ob.CompareTag("Slash"))
        {
            Instantiate(Resources.Load("Prefabs/VFX/EnemyDeath"), transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }

        if (ob.CompareTag("Arrow") && ob.GetComponent<Arrow>().Emited)
        {
            ActivateStunEffect();
            GetComponent<AudioSource>().Play();
            GetComponent<Enemy_Check>().stun_time_count = 0;
            status = EnemyStatus.Stunned;
            GetComponent<Enemy_Check>().RemoveLaser();
        }
    }

    private void ActivateStunEffect()
    {
        GameObject Effect = transform.Find("StunnedEffect").gameObject;
        Effect.GetComponent<ParticleSystem>().Play(true);
    }

    private void DeActivateStunEffect()
    {
        GameObject Effect = transform.Find("StunnedEffect").gameObject;
        Effect.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
