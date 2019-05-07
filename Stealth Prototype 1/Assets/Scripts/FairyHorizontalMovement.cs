using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class FairyHorizontalMovement : MonoBehaviour
{
    public bool Fast;

    public float FastHorizontalSpeed;
    public float SlowHorizontalSpeed;

    public float AirAcceleration;

    private Player player;

    private const float FastMoveVectorThreshold = 1f;
    private const float SlowMoveVectorThreshold = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        player = ControllerManager.Fairy;

    }

    // Update is called once per frame
    void Update()
    {
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (Fairy_Status.status==FairyStatus.Normal||Fairy_Status.status==FairyStatus.Float || Fairy_Status.status==FairyStatus.Aiming)
        {
            check_input();
        }
    }

    private void check_input()
    {
        var check_horizontal_collider = GetComponent<CheckHorizontalCollider>();
        var CharacterMove = GetComponent<CharacterMove>();

        Vector3 moveVector = Vector3.zero;
        moveVector.x = player.GetAxis("Left Stick X");
        float CurrentMaxSpeed;
        float CurrentAirAceleration;

        if (Mathf.Abs(moveVector.x) < SlowMoveVectorThreshold)
        {
            CurrentMaxSpeed = 0;
            CurrentAirAceleration = 0;
            moveVector.x = 0;
            Fast = false;
        }
        else if(Mathf.Abs(moveVector.x) < FastMoveVectorThreshold)
        {
            CurrentMaxSpeed = SlowHorizontalSpeed;
            CurrentAirAceleration = AirAcceleration;
            moveVector.Normalize();
            Fast = false;
        }
        else
        {
            CurrentMaxSpeed = FastHorizontalSpeed;
            CurrentAirAceleration = AirAcceleration;
            moveVector.Normalize();
            Fast = true;
        }

        if (GetComponent<Fairy_Status_Manager>().status != FairyStatus.Aiming)
        {
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



        if ( moveVector.x > 0 && !CharacterMove.HitRightWall || moveVector.x < 0 && !CharacterMove.HitLeftWall)
        {
            if (CharacterMove.OnGround)
            {
                if (moveVector.x > 0)
                {
                    CharacterMove.speed.x = CurrentMaxSpeed;
                }
                else
                {
                    CharacterMove.speed.x = -CurrentMaxSpeed;
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
                    CharacterMove.speed.x += CurrentAirAceleration * Time.deltaTime;
                    if (CharacterMove.speed.x > CurrentMaxSpeed)
                    {
                        CharacterMove.speed.x = CurrentMaxSpeed;
                    }
                }
                else
                {
                    if (CharacterMove.speed.x > 0)
                    {
                        CharacterMove.speed.x = 0;
                    }
                    CharacterMove.speed.x -= CurrentAirAceleration * Time.deltaTime;
                    if (CharacterMove.speed.x < -CurrentMaxSpeed)
                    {
                        CharacterMove.speed.x = -CurrentMaxSpeed;
                    }
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
