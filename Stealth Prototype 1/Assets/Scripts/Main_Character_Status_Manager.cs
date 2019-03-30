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
    Aimed
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
    // Start is called before the first frame update
    void Start()
    {
        player= GetComponent<PlayerId>().player;
        EventManager.instance.AddHandler<CharacterDied>(OnCharacterDied);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CharacterDied>(OnCharacterDied);
    }
    // Update is called once per frame
    void Update()
    {
        SetInvisibility();
        check_aimed();
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

    private void set_status()
    {
        if (status == MainCharacterStatus.Normal)
        {
            GetComponent<Invisible>().AbleToInvisible = true;
        }
        else if (status == MainCharacterStatus.Dashing)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
        }
        else if (status == MainCharacterStatus.Climbing)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
        }
        else if (status == MainCharacterStatus.Transporting)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
        }
        else if (status == MainCharacterStatus.Aimed)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
        }
    }

    private void check_aimed()
    {
        if (status == MainCharacterStatus.Aimed)
        {
            player.SetVibration(0, AimedVibration, Time.deltaTime);
            AimedTimeCount += Time.deltaTime;
            if (AimedTimeCount > AimedDiedTime)
            {
                EventManager.instance.Fire(new CharacterDied(gameObject));
                Destroy(gameObject);
            }
        }
        else
        {
            AimedTimeCount = 0;
        }
    }

    private void OnCharacterDied(CharacterDied C)
    {
        if (C.DeadCharacter == gameObject)
        {
            player.SetVibration(0, DeadVibration, DeadVibrationTime);
        }
    }
}
