using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Fairy_Status_Manager : MonoBehaviour
{
    public int status;

    public int NORMAL = 0;
    public int FLOAT = 1;
    public int AIM = 2;
    public int CLIMBING = 3;
    public int FLOAT_PLATFORM = 4;
    public int TRANSPORTING = 5;
    public int AIMED = 6;

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
        SetPhysicsData();
        SetInvisibility();
        FloatGoingDown();
        set_status();
        check_aimed();
    }

    private void SetPhysicsData()
    {
        if (!Freeze_Manager.freeze)
        {
            if (status == NORMAL || status == AIM || status == TRANSPORTING)
            {
                GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
            }
            else if (status == FLOAT || status == FLOAT_PLATFORM || status == CLIMBING || status == AIMED)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    private void SetInvisibility()
    {
        if (status == NORMAL)
        {
            GetComponent<Invisible>().AbleToInvisible = true;
        }
        else if (status == FLOAT || status == FLOAT_PLATFORM || status == CLIMBING || status == AIM || status == TRANSPORTING || status == AIMED)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
        }
    }

    private void FloatGoingDown()
    {
        if (status == FLOAT&&!Freeze_Manager.freeze)
        {
            transform.position += Vector3.down * GetComponent<Gravity_Data>().float_down_speed * Time.deltaTime;
        }
    }

    private void set_status()
    {
        Color current_color = GetComponent<SpriteRenderer>().color;
        if (status == NORMAL)
        {
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == FLOAT)
        {
            
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, current_color.a);
            transform.parent = null;
        }
        else if (status == AIM)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, current_color.a);
        }
        else if (status == CLIMBING)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == FLOAT_PLATFORM)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().color = new Color(100 / 255f, 1, 0, current_color.a);
            transform.parent = null;
        }
        else if (status == TRANSPORTING)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == AIMED)
        {
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
            transform.parent = null;
        }
    }

    private void check_aimed()
    {
        if (status == AIMED)
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
