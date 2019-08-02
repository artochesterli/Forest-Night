using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Climb : MonoBehaviour
{

    public float ClimbSpeed;
    public Vector2 JumpOffSpeed;
    public float ThroughPathEndDuration;

    private Player player;
    private bool IsClimbing;
    public bool PathEndThrough;


    private bool InPathEnd;
    private bool AbleToClimb;
    public GameObject ConnectedPath;

    private const float ClimbThreshold = 0.9f;
    private const float JumpOffThreshold = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            player = ControllerManager.Fairy;
        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            player = ControllerManager.MainCharacter;
        }

        ConnectedPath = null;
        InPathEnd = false;
        IsClimbing = false;
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            var Status = GetComponent<Fairy_Status_Manager>();
            if ((Status.status==FairyStatus.Normal|| Status.status==FairyStatus.Float || Status.status==FairyStatus.Climbing) && !PathEndThrough)
            {
                Check_Input();
            }
            else if (Status.status == FairyStatus.Aimed)
            {
                IsClimbing = false;
            }

        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            var Status = GetComponent<Main_Character_Status_Manager>();
            if ((Status.status == MainCharacterStatus.Normal || Status.status==MainCharacterStatus.Climbing) && !PathEndThrough)
            {
                Check_Input();
            }
            else if (Status.status == MainCharacterStatus.Aimed)
            {
                IsClimbing = false;
            }
        }
        
        Check_Status();
    }


    private void Check_Input()
    {
        if ( ConnectedPath!=null && AbleToClimb && !IsClimbing && (InputUp() && ConnectedPath.transform.position.y>transform.position.y|| InputDown()&& ConnectedPath.transform.position.y < transform.position.y))
        {
            IsClimbing = true;
            if (gameObject.CompareTag("Fairy"))
            {
                var Status = GetComponent<Fairy_Status_Manager>();
                Status.status = FairyStatus.Climbing;

            }
            else if (gameObject.CompareTag("Main_Character"))
            {
                var Status = GetComponent<Main_Character_Status_Manager>();
                Status.status = MainCharacterStatus.Climbing;
            }

            if (InputUp())
            {
                transform.position = new Vector3(ConnectedPath.transform.position.x, transform.position.y, 0);
            }
            else
            {
                transform.position = new Vector3(ConnectedPath.transform.position.x, transform.position.y, 0);
            }

            if (InPathEnd)
            {
                if(InputUp())
                {
                    StartCoroutine(ThroughPathEnd(true));
                }
                else
                {
                    StartCoroutine(ThroughPathEnd(false));
                }
                return;
            }
        }

        if (!AbleToClimb)
        {
            IsClimbing = false;
        }

        if(IsClimbing)
        {
            transform.position = new Vector3(ConnectedPath.transform.position.x, transform.position.y, transform.position.z);
            GetComponent<CharacterMove>().speed.x = 0;
            if (InputUp()||InputDown())
            {
                if (InputUp())
                {
                    GetComponent<CharacterMove>().speed.y = ClimbSpeed;
                }
                else
                {
                    GetComponent<CharacterMove>().speed.y = -ClimbSpeed;
                }

                if (InPathEnd && !PathEndThrough)
                {
                    if (InputUp())
                    {
                        StartCoroutine(ThroughPathEnd(true));
                    }
                    else
                    {
                        StartCoroutine(ThroughPathEnd(false));
                    }
                }
            }
            else
            {
                GetComponent<CharacterMove>().speed.y = 0;
            }

            if (InputRight())
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
            }
            else if(InputLeft())
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.Find("LightToEnvironment").rotation = Quaternion.Euler(0, 0, 0);
            }

            if (InputJumpOff())
            {
                IsClimbing = false;
                if (transform.right.x > 0)
                {
                    GetComponent<CharacterMove>().speed = new Vector2(JumpOffSpeed.x, JumpOffSpeed.y);
                }
                else
                {
                    GetComponent<CharacterMove>().speed = new Vector2(-JumpOffSpeed.x, JumpOffSpeed.y);
                }
            }
        }


    }

    private void Check_Status()
    {

        if (!IsClimbing)
        {
            if (gameObject.CompareTag("Fairy"))
            {
                var Status = GetComponent<Fairy_Status_Manager>();
                if (Status.status == FairyStatus.Climbing)
                {
                    Status.status = FairyStatus.Normal;
                }

            }
            else if (gameObject.CompareTag("Main_Character"))
            {
                var Status = GetComponent<Main_Character_Status_Manager>();
                if (Status.status == MainCharacterStatus.Climbing)
                {
                    Status.status = MainCharacterStatus.Normal;
                }
            }
        }

        if (!InPathEnd)
        {
            if (GetComponent<CharacterMove>().OnGround)
            {
                IsClimbing = false;
            }
        }

    }

    private bool InputJumpOff()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            if (ControllerManager.FairyJoystick != null)
            {
                return player.GetButtonDown("A");
            }
            else
            {
                return Input.GetKey(KeyCode.Space);
            }
        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            if (ControllerManager.MainCharacterJoystick != null)
            {
                return player.GetButtonDown("A");
            }
            else
            {
                return Input.GetKey(KeyCode.RightControl);
            }
        }
        else
        {
            return false;
        }
    }

    private bool InputRight()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            if (ControllerManager.FairyJoystick != null)
            {
                return player.GetAxis("Left Stick X") > JumpOffThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A);
            }
        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            if (ControllerManager.MainCharacterJoystick != null)
            {
                return player.GetAxis("Left Stick Y") > JumpOffThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow);
            }
        }
        else
        {
            return false;
        }
    }

    private bool InputLeft()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            if (ControllerManager.FairyJoystick != null)
            {
                return player.GetAxis("Left Stick Y") < -JumpOffThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D);
            }
        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            if (ControllerManager.MainCharacterJoystick != null)
            {
                return player.GetAxis("Left Stick Y") > -JumpOffThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow);
            }
        }
        else
        {
            return false;
        }
    }

    private bool InputUp()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            if (ControllerManager.FairyJoystick != null)
            {
                return player.GetAxis("Left Stick Y") > ClimbThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S);
            }
        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            if (ControllerManager.MainCharacterJoystick != null)
            {
                return player.GetAxis("Left Stick Y") > ClimbThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
            }
        }
        else
        {
            return false;
        }
    }

    private bool InputDown()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            if (ControllerManager.FairyJoystick != null)
            {
                return player.GetAxis("Left Stick Y") < -ClimbThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W);
            }
        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            if (ControllerManager.MainCharacterJoystick != null)
            {
                return player.GetAxis("Left Stick Y") < -ClimbThreshold;
            }
            else
            {
                return Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow);
            }
        }
        else
        {
            return false;
        }
    }
    private IEnumerator ThroughPathEnd(bool up)
    {
        GameObject PathEnd = ConnectedPath.transform.Find("Path_End").gameObject;
        if(up && PathEnd.transform.position.y<transform.position.y || !up && PathEnd.transform.position.y > transform.position.y)
        {
            yield break;
        }

        PathEndThrough = true;
        GetComponent<CharacterMove>().speed = Vector2.zero;
        Vector3 Target;
        if (up)
        {
            Target = PathEnd.transform.position + Vector3.up;
        }
        else
        {
            Target = PathEnd.transform.position + Vector3.down;
        }
        Vector3 speed = (Target - transform.position) / ThroughPathEndDuration;
        float timecount = 0;
        while (timecount < ThroughPathEndDuration)
        {
            transform.position += speed * Time.deltaTime;
            timecount += Time.deltaTime;
            yield return null;
        }
        transform.position = Target;
        if (up)
        {
            IsClimbing = false;
        }
        PathEndThrough = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Path"))
        {
            ConnectedPath = ob;
            AbleToClimb = true;
        }
        if (ob.name == "Path_End")
        {
            ConnectedPath = ob.transform.parent.gameObject;
            AbleToClimb = true;
            InPathEnd = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Path"))
        {
            ConnectedPath = null;
            AbleToClimb = false;
        }
        if (ob.name == "Path_End")
        {
            InPathEnd = false;
            if (transform.position.y > ob.transform.position.y)
            {
                ConnectedPath = null;
                AbleToClimb = false;
            }
        }
    }

    private void OnLoadLevel(LoadLevel L)
    {
        StopAllCoroutines();
        InPathEnd = false;
        ConnectedPath = null;
        AbleToClimb = false;
        PathEndThrough = false;
        IsClimbing = false;
        
    }
}
