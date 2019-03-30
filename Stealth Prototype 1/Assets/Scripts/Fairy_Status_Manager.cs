using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum FairyStatus
{
    Normal,
    Float,
    FloatPlatform,
    Aiming,
    Climbing,
    Transporting,
    Aimed
}

public class Fairy_Status_Manager : MonoBehaviour
{
    public FairyStatus status;

    private float AimedTimeCount;
    private Player player;

    private const float AimedDiedTime = 1;

    private const float AimedVibration = 0.1f;
    private const float DeadVibration = 1.0f;
    private const float DeadVibrationTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
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
        FloatGoingDown();
        set_status();
        check_aimed();
    }

    private void SetInvisibility()
    {
        if (status == FairyStatus.Normal)
        {
            GetComponent<Invisible>().AbleToInvisible = true;
        }
        else if (status == FairyStatus.Float || status == FairyStatus.FloatPlatform || status == FairyStatus.Climbing || status == FairyStatus.Aiming || status == FairyStatus.Transporting || status == FairyStatus.Aimed)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
        }
    }

    private void FloatGoingDown()
    {
        if (status == FairyStatus.Float&&!Freeze_Manager.freeze)
        {
            GetComponent<CharacterMove>().speed.y = -GetComponent<Gravity_Data>().float_down_speed;
        }
    }

    private void set_status()
    {
        Color current_color = GetComponent<SpriteRenderer>().color;
        if (status == FairyStatus.Normal)
        {
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == FairyStatus.Float)
        {
            
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, current_color.a);
            transform.parent = null;
        }
        else if (status == FairyStatus.Aiming)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, current_color.a);
        }
        else if (status == FairyStatus.Climbing)
        {
            GetComponent<CharacterMove>().speed = Vector2.zero;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == FairyStatus.FloatPlatform)
        {
            GetComponent<CharacterMove>().speed = Vector2.zero;
            GetComponent<SpriteRenderer>().color = new Color(100 / 255f, 1, 0, current_color.a);
            transform.parent = null;
        }
        else if (status == FairyStatus.Transporting)
        {
            GetComponent<CharacterMove>().speed = Vector2.zero;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == FairyStatus.Aimed)
        {
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
            transform.parent = null;
        }
    }

    private void check_aimed()
    {
        if (status == FairyStatus.Aimed)
        {
            player.SetVibration(1, AimedVibration, Time.deltaTime);
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
            player.SetVibration(1, DeadVibration, DeadVibrationTime);
        }
    }
}
