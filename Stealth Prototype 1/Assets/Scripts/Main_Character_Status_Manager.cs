using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Main_Character_Status_Manager : MonoBehaviour
{
    public int status;

    public int NORMAL = 0;
    public int DASHING = 1;
    public int CLIMBING = 2;
    public int TRANSPORTING = 3;
    public int AIMED = 4;

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
        set_status();
        check_aimed();
    }

    private void set_status()
    {
        if (status == NORMAL)
        {
            GetComponent<Invisible>().AbleToInvisible = true;
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
        }
        else if (status == DASHING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
           // GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else if (status == CLIMBING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else if (status == TRANSPORTING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
        }
        else if (status == AIMED)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
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
