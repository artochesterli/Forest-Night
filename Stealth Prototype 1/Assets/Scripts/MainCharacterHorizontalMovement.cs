using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainCharacterHorizontalMovement : MonoBehaviour
{
    public float HorizontalSpeed;
    public float AirAcceleration;

    private Player player;

    private const float MoveThreshold = 0.4f;
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
            CheckInput();
        }
    }

    private bool InputRight()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return player.GetAxis("Left Stick X") > MoveThreshold;
        }
        else
        {
            return Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow);
        }
    }

    private bool InputLeft()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            return player.GetAxis("Left Stick X") < -MoveThreshold;
        }
        else
        {
            return Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow);
        }
    }

   

    private void CheckInput()
    {
        var CharacterMove = GetComponent<CharacterMove>();

        if (InputRight())
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (InputLeft())
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
        }

        if (InputRight() && !CharacterMove.HitRightWall || InputLeft() && !CharacterMove.HitLeftWall)
        {
            if (CharacterMove.OnGround)
            {
                if (InputRight())
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
                if (InputRight())
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
