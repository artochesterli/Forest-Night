using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainCharacterHorizontalMovement : MonoBehaviour
{
    public float HorizontalSpeed;
    public float AirAcceleration;

    private Player player;

    private const float moveVectorThreshold = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        player = ControllerManager.MainCharacter;

    }

    // Update is called once per frame
    void Update()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (Main_Character_Status.status == MainCharacterStatus.Normal || Main_Character_Status.status == MainCharacterStatus.OverDash)
        {
            check_input();
        }
    }

    private void check_input()
    {
        var CharacterMove = GetComponent<CharacterMove>();

        Vector3 moveVector = Vector3.zero;
        moveVector.x = player.GetAxis("Left Stick X");

        if (Mathf.Abs(moveVector.x) < moveVectorThreshold)
        {
            moveVector.x = 0;
        }
        else
        {
            moveVector.Normalize();
        }


        if (moveVector.x > 0 && !CharacterMove.HitRightWall || moveVector.x < 0 && !CharacterMove.HitLeftWall)
        {
            if (CharacterMove.OnGround)
            {
                if (moveVector.x > 0)
                {
                    CharacterMove.speed.x = HorizontalSpeed;
                }
                else
                {
                    CharacterMove.speed.x = -HorizontalSpeed;
                }

            }
            else
            {
                if (moveVector.x > 0)
                {
                    if (CharacterMove.speed.x < 0)
                    {
                        CharacterMove.speed.x = 0;
                    }
                    CharacterMove.speed.x += AirAcceleration * Time.deltaTime;
                    if (CharacterMove.speed.x > HorizontalSpeed)
                    {
                        CharacterMove.speed.x = HorizontalSpeed;
                    }
                }
                else
                {
                    if (CharacterMove.speed.x > 0)
                    {
                        CharacterMove.speed.x = 0;
                    }
                    CharacterMove.speed.x -= AirAcceleration * Time.deltaTime;
                    if (CharacterMove.speed.x < -HorizontalSpeed)
                    {
                        CharacterMove.speed.x = -HorizontalSpeed;
                    }
                }
            }

            if (moveVector.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (moveVector.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (CharacterMove.OnGround)
            {
                CharacterMove.speed.x = 0;
            }
            else
            {
                if (CharacterMove.speed.x > 0)
                {
                    CharacterMove.speed.x -= AirAcceleration * Time.deltaTime;
                    if (CharacterMove.speed.x < 0)
                    {
                        CharacterMove.speed.x = 0;
                    }
                }
                else
                {
                    CharacterMove.speed.x += AirAcceleration * Time.deltaTime;
                    if (CharacterMove.speed.x > 0)
                    {
                        CharacterMove.speed.x = 0;
                    }
                }
            }
        }

    }
}
