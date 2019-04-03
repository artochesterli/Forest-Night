using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Horizontal_Movement : MonoBehaviour
{
    public float HorizontalSpeed;
    public float AirAcceleration;

    private Player player;

    private const float moveVectorThreshold = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            var Fairy_Status = GetComponent<Fairy_Status_Manager>();
            if (Fairy_Status.status != FairyStatus.Climbing&&Fairy_Status.status!=FairyStatus.FloatPlatform&&Fairy_Status.status!=FairyStatus.Transporting&&Fairy_Status.status!=FairyStatus.Aimed && Fairy_Status.status!=FairyStatus.KnockBack)
            {
                check_input();
            }
        }
        else if(gameObject.CompareTag("Main_Character"))
        {
            var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
            if (Main_Character_Status.status == MainCharacterStatus.Normal || Main_Character_Status.status==MainCharacterStatus.OverDash)
            {
                check_input();
            }
            
        }
    }

    private void check_input()
    {
        var check_horizontal_collider = GetComponent<CheckHorizontalCollider>();
        var CharacterMove = GetComponent<CharacterMove>();

        Vector3 moveVector=Vector3.zero;
        moveVector.x = player.GetAxis("Left Stick X");
        if (Mathf.Abs(moveVector.x) < moveVectorThreshold)
        {
            moveVector.x = 0;
        }
        else
        {
            moveVector.Normalize();
        }


        if (Mathf.Abs(moveVector.x) > 0 && (!CharacterMove.HitWall || moveVector.x > 0 && CharacterMove.WallDirection.x < 0 || moveVector.x < 0 && CharacterMove.WallDirection.x > 0))
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

            if (!(gameObject.CompareTag("Fairy") && GetComponent<Fairy_Status_Manager>().status == FairyStatus.Aiming))
            {
                if (moveVector.x > 0)
                {
                    transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                }
                else if (moveVector.x < 0)
                {
                    transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                }
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
