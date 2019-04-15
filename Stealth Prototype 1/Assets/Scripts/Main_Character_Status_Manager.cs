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
    KnockBack
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
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CharacterDied>(OnCharacterDied);
        EventManager.instance.RemoveHandler<CharacterHitSpineEdge>(OnCharacterHitSpineEdge);
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

    private void check_aimed()
    {
        if (status == MainCharacterStatus.Aimed)
        {
            GetComponent<CharacterMove>().speed = Vector2.zero;
            player.SetVibration(0, AimedVibration, Time.deltaTime);
            AimedTimeCount += Time.deltaTime;
            if (AimedTimeCount > AimedDiedTime)
            {
                EventManager.instance.Fire(new CharacterDied(gameObject));
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
            player.SetVibration(0, KnockBackVibration, KnockBackVibrationTime);
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
