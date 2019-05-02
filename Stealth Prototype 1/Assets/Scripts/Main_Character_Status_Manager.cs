using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum MainCharacterStatus
{
    Normal,
    Dashing,
    OverDash,
    Climbing,
    Transporting,
    Aimed,
    KnockBack,
}

public class Main_Character_Status_Manager : MonoBehaviour
{
    public MainCharacterStatus status;

    private float AimedTimeCount;
    private Player player;

    private const float AimedDiedTime = 1;

    private const float AimedVibration = 0.1f;
    private const float DeadVibration = 1.0f;
    private const float DeadVibrationTime = 0.2f;
    private const float KnockBackVibration = 0.5f;
    private const float KnockBackVibrationTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        player= GetComponent<PlayerId>().player;
        EventManager.instance.AddHandler<CharacterDied>(OnCharacterDied);
        EventManager.instance.AddHandler<CharacterHitSpineEdge>(OnCharacterHitSpineEdge);
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CharacterDied>(OnCharacterDied);
        EventManager.instance.RemoveHandler<CharacterHitSpineEdge>(OnCharacterHitSpineEdge);
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }
    // Update is called once per frame
    void Update()
    {
        SetInvisibility();
        SetAudio();
        SetDashEffect();
        CheckAimed();
    }

    private void SetInvisibility()
    {
        if(status == MainCharacterStatus.Normal && GetComponent<CharacterMove>().OnGround)
        {
            GetComponent<Invisible>().AbleToInvisible = true;
        }
        else
        {
            GetComponent<Invisible>().AbleToInvisible = false;
        }
    }

    private void SetAudio()
    {
        if(status==MainCharacterStatus.Normal && GetComponent<CharacterMove>().OnGround && GetComponent<CharacterMove>().speed.x!=0)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    private void SetDashEffect()
    {
        GameObject DashEffect = transform.Find("DashEffect").gameObject;
        if(status==MainCharacterStatus.Dashing || status == MainCharacterStatus.OverDash)
        {
            if (!DashEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                DashEffect.GetComponent<ParticleSystem>().Play(true);
            }
        }
        else
        {
            DashEffect.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void CheckAimed()
    {
        if (status == MainCharacterStatus.Aimed)
        {
            GetComponent<CharacterMove>().speed = Vector2.zero;
            player.SetVibration(0, AimedVibration, Time.deltaTime);
            AimedTimeCount += Time.deltaTime;
            if (AimedTimeCount > AimedDiedTime)
            {
                if (Character_Manager.Main_Character.activeSelf && Character_Manager.Fairy.activeSelf)
                {
                    EventManager.instance.Fire(new CharacterDied(gameObject));
                    
                }
                gameObject.SetActive(false);
            }
        }
        else
        {
            AimedTimeCount = 0;
        }
    }

    private void OnCharacterHitSpineEdge(CharacterHitSpineEdge C)
    {
        if (C.Character == gameObject)
        {
            status = MainCharacterStatus.KnockBack;
            player.SetVibration(0, KnockBackVibration, KnockBackVibrationTime);
        }
    }

    private void OnCharacterDied(CharacterDied C)
    {
        if (C.DeadCharacter == gameObject)
        {
            Instantiate(Resources.Load("Prefabs/VFX/BigGuyDeath"), transform.position, Quaternion.Euler(0, 0, 0));
            player.SetVibration(0, DeadVibration, DeadVibrationTime);
        }
    }

    private void OnLoadLevel(LoadLevel L)
    {
        AimedTimeCount = 0;
        status = MainCharacterStatus.Normal;
    }
}
