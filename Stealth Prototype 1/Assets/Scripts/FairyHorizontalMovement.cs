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

    private const float FastMoveVectorThreshold = 0.95f;
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
            CheckInput();
        }
    }

    private bool InputRightFast()
    {
        if (ControllerManager.FairyJoystick != null)
        {
            return player.GetAxis("Left Stick X") >= FastMoveVectorThreshold;
        }
        else
        {
            return Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftAlt) &&!Input.GetKey(KeyCode.A);
        }
    }

    private bool InputLeftFast()
    {
        if (ControllerManager.FairyJoystick != null)
        {
            return player.GetAxis("Left Stick X") <= -FastMoveVectorThreshold;
        }
        else
        {
            return Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.D);
        }
    }

    private bool InputRightSlow()
    {
        if (ControllerManager.FairyJoystick != null)
        {
            return player.GetAxis("Left Stick X") >= SlowMoveVectorThreshold && player.GetAxis("Left Stick X") < FastMoveVectorThreshold;
        }
        else
        {
            return Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.A);
        }
    }

    private bool InputLeftSlow()
    {
        if (ControllerManager.FairyJoystick != null)
        {
            return player.GetAxis("Left Stick X") <= -SlowMoveVectorThreshold && player.GetAxis("Left Stick X") > -FastMoveVectorThreshold;
        }
        else
        {
            return Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.D);
        }
    }

    private void CheckInput()
    {
        var check_horizontal_collider = GetComponent<CheckHorizontalCollider>();
        var CharacterMove = GetComponent<CharacterMove>();

        bool right = false;
        float CurrentMaxSpeed;
        float CurrentAirAceleration;

        if (!InputRightFast()&&!InputLeftFast()&&!InputRightSlow()&&!InputLeftSlow())
        {
            CurrentMaxSpeed = 0;
            CurrentAirAceleration = 0;
            Fast = false;
        }
        else if(InputRightSlow()||InputLeftSlow())
        {
            CurrentMaxSpeed = SlowHorizontalSpeed;
            CurrentAirAceleration = AirAcceleration;
            if (InputRightSlow())
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (GetComponent<Fairy_Status_Manager>().status != FairyStatus.Aiming)
            {
                if (right)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            Fast = false;
        }
        else
        {
            CurrentMaxSpeed = FastHorizontalSpeed;
            CurrentAirAceleration = AirAcceleration;
            if (InputRightFast())
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (GetComponent<Fairy_Status_Manager>().status != FairyStatus.Aiming)
            {
                if (right)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            Fast = true;
        }


        if ( right && !CharacterMove.HitRightWall || !right && !CharacterMove.HitLeftWall)
        {
            if (CharacterMove.OnGround)
            {
                if (right)
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
                if (right)
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
