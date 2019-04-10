using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public Vector2 speed;
    public Vector2 DashSpeed;
    public Vector2 PlatformSpeed;
    public float Gravity;

    public bool HitRightWall;
    public float RightWallDis;
    public GameObject RightWall;

    public bool HitLeftWall;
    public float LeftWallDis;
    public GameObject LeftWall;


    public bool HitTop;
    public float TopDis;
    public GameObject Top;

    public bool OnGround;
    public float GroundDis;
    public GameObject Ground;

    public GameObject ConnectedMovingPlatform;
    private int ConnectedPlatformMoveFrameCount;
    private int layermask;


    public float OnGroundThreshold;
    public float OnGroundDetectOffset;

    public float HitWallThreshold;
    public float HitWallDetectOffset;


    public float HitTopThreshold;
    public float HitTopDetectOffset;

    private const float DetectDis = 2;
    private const float CheckOffset = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Fairy") | 1 << LayerMask.NameToLayer("Path") | 1 << LayerMask.NameToLayer("Gem") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Portal") | 1<<LayerMask.NameToLayer("Arrow");
        layermask = ~layermask;
        EventManager.instance.AddHandler<ConnectedPlatformMoved>(OnConnectedPlatformMoved);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ConnectedPlatformMoved>(OnConnectedPlatformMoved);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckGroundDis();
        CheckLeftWallDis();
        CheckRightWallDis();
        CheckTopDis();
        CheckGroundHitting();
        CheckLeftWallHitting();
        CheckRightWallHitting();
        CheckTopHitting();


        SetGravity();
        GravityEffect();
        RectifySpeed();
        Move();
        RectifyPos();
    }

    private void SetGravity()
    {
        if (!Freeze_Manager.freeze)
        {
            if (gameObject.CompareTag("Main_Character"))
            {
                MainCharacterStatus status = GetComponent<Main_Character_Status_Manager>().status;
                if ((status == MainCharacterStatus.Normal || status==MainCharacterStatus.KnockBack) && !OnGround)
                {
                    Gravity = GetComponent<Gravity_Data>().normal_gravityScale;
                }
                else
                {
                    Gravity = 0;
                }
            }
            else if (gameObject.CompareTag("Fairy"))
            {
                FairyStatus status = GetComponent<Fairy_Status_Manager>().status;
                if ((status == FairyStatus.Normal || status==FairyStatus.KnockBack) && !OnGround)
                {
                    Gravity = GetComponent<Gravity_Data>().normal_gravityScale;
                }
                else
                {
                    Gravity = 0;
                }
            }
        }
        else
        {
            Gravity = 0;
        }

    }

    private void RectifySpeed()
    {
        
        if (!IsMainCharacterDashing())
        {
            if (HitRightWall && speed.x>0 || HitLeftWall && speed.x<0)
            {
                speed.x = 0;
            }
            if (HitTop && speed.y > 0 || OnGround && speed.y < 0)
            {
                speed.y = 0;
            }
        }
        else
        {
            PlatformSpeed = Vector2.zero;
        }
        if (IsCharacterAimed())
        {
            speed = Vector2.zero;
            DashSpeed = Vector2.zero;
        }
        if (IsFairyIgnorePlatformSpeed())
        {
            PlatformSpeed = Vector2.zero;
        }
    }

    private void GravityEffect()
    {
        speed.y -= Gravity * 10 * Time.deltaTime;
    }

    public void Move()
    {
        Vector2 temp = speed+DashSpeed + PlatformSpeed;
        if (!IsMainCharacterDashing())
        {
            if (temp.y > 0 && TopDis >= 0 && temp.y * Time.deltaTime > TopDis)
            {
                temp.y = TopDis / Time.deltaTime;
                if (IsMainCharacterOverDashing())
                {
                    GetComponent<Main_Character_Status_Manager>().status = MainCharacterStatus.Normal;
                }
                DashSpeed = Vector2.zero;
            }

            if (temp.y < 0 && GroundDis >= 0 && temp.y * Time.deltaTime < -GroundDis)
            {
                temp.y = -GroundDis / Time.deltaTime;
                if (IsMainCharacterOverDashing())
                {
                    GetComponent<Main_Character_Status_Manager>().status = MainCharacterStatus.Normal;
                }
                DashSpeed = Vector2.zero;
            }

            if(temp.x > 0 && RightWallDis>=0 && temp.x * Time.deltaTime > RightWallDis)
            {
                temp.x = RightWallDis/Time.deltaTime;
                if (IsMainCharacterOverDashing())
                {
                    GetComponent<Main_Character_Status_Manager>().status = MainCharacterStatus.Normal;
                }
                DashSpeed = Vector2.zero;
            }

            if(temp.x<0 && LeftWallDis>=0 && temp.x * Time.deltaTime < -LeftWallDis)
            {
                temp.x = -LeftWallDis / Time.deltaTime;
                if (IsMainCharacterOverDashing())
                {
                    GetComponent<Main_Character_Status_Manager>().status = MainCharacterStatus.Normal;
                }
                DashSpeed = Vector2.zero;
            }
        }
        
        transform.position += (Vector3)temp* Time.deltaTime;
        if (ConnectedMovingPlatform != null)
        {
            EventManager.instance.Fire(new CharacterMoveWithPlatform(Time.frameCount, gameObject));
        }
    }

    public void CheckGroundDis()
    {
        

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.right * OnGroundDetectOffset, Vector2.down, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.left * OnGroundDetectOffset, Vector2.down, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.y - transform.position.y) < Mathf.Abs(hit2.point.y - transform.position.y))
            {
                GroundDis = Mathf.Abs(hit1.point.y - transform.position.y) - OnGroundThreshold;
                Ground = hit1.collider.gameObject;
            }
            else
            {
                GroundDis = Mathf.Abs(hit2.point.y - transform.position.y) - OnGroundThreshold;
                Ground = hit2.collider.gameObject;
            }
        }
        else if (hit1)
        {
            GroundDis = Mathf.Abs(hit1.point.y - transform.position.y) - OnGroundThreshold;
            Ground = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            GroundDis = Mathf.Abs(hit2.point.y - transform.position.y) - OnGroundThreshold;
            Ground = hit2.collider.gameObject;
        }
        else
        {
            GroundDis = System.Int32.MaxValue;
            Ground = null;
        }
        if (Ground != null && (Ground.CompareTag("Platform_Totem") || Ground.CompareTag("Totem_Platform")))
        {
            
            GroundDis -= PlatformSpeed.y * Time.deltaTime;
        }

    }

    private void CheckGroundHitting()
    {
        float Dis = CheckOffset;
        if (Ground != null && (Ground.CompareTag("Platform_Totem") || Ground.CompareTag("Totem_Platform")))
        {
            Dis -= PlatformSpeed.y * Time.deltaTime;
        }
        if (GroundDis <= Dis)
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
    }

    public void CheckTopDis()
    {

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.right * HitTopDetectOffset, Vector2.up, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.left * HitTopDetectOffset, Vector2.up, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.y - transform.position.y) < Mathf.Abs(hit2.point.y - transform.position.y))
            {
                TopDis = Mathf.Abs(hit1.point.y - transform.position.y) - HitTopThreshold;
                Top = hit1.collider.gameObject;
            }
            else
            {
                TopDis = Mathf.Abs(hit2.point.y - transform.position.y) - HitTopThreshold;
                Top = hit2.collider.gameObject;
            }
            
        }
        else if (hit1)
        {
            TopDis = Mathf.Abs(hit1.point.y - transform.position.y) - HitTopThreshold;
            Top = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            TopDis = Mathf.Abs(hit2.point.y - transform.position.y) - HitTopThreshold;
            Top = hit2.collider.gameObject;
        }
        else
        {
            TopDis = System.Int32.MaxValue;
            Top = null;
        }
    }

    private void CheckTopHitting()
    {
        if (TopDis <= CheckOffset)
        {
            HitTop = true;
        }
        else
        {
            HitTop = false;
        }
    }

    public void CheckLeftWallDis()
    {

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.up * HitWallDetectOffset, Vector2.left, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.down * HitWallDetectOffset, Vector2.left, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.x - transform.position.x) < Mathf.Abs(hit2.point.x - transform.position.x))
            {
                LeftWallDis = Mathf.Abs(hit1.point.x - transform.position.x) - HitWallThreshold;
                LeftWall = hit1.collider.gameObject;
            }
            else
            {
                LeftWallDis = Mathf.Abs(hit2.point.x - transform.position.x) - HitWallThreshold;
                LeftWall = hit2.collider.gameObject;
            }
        }
        else if (hit1)
        {
            LeftWallDis = Mathf.Abs(hit1.point.x - transform.position.x) - HitWallThreshold;
            LeftWall = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            LeftWallDis = Mathf.Abs(hit2.point.x - transform.position.x) - HitWallThreshold;
            LeftWall = hit2.collider.gameObject;
        }
        else
        {
            LeftWallDis = System.Int32.MaxValue;
            LeftWall = null;
        }
        if (LeftWall != null && LeftWall.CompareTag("Platform_Totem"))
        {
            LeftWallDis -= PlatformSpeed.x * Time.deltaTime;
        }
    }

    private void CheckLeftWallHitting()
    {
        float Dis = CheckOffset;
        if (LeftWall != null && LeftWall.CompareTag("Platform_Totem"))
        {
            Dis -= PlatformSpeed.x * Time.deltaTime;
        }
        if (LeftWallDis <= Dis)
        {
            HitLeftWall = true;
        }
        else
        {
            HitLeftWall = false;
        }
    }

    public void CheckRightWallDis()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.up * HitWallDetectOffset, Vector2.right, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.down * HitWallDetectOffset, Vector2.right, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if(Mathf.Abs(hit1.point.x - transform.position.x)< Mathf.Abs(hit2.point.x - transform.position.x))
            {
                RightWallDis = Mathf.Abs(hit1.point.x - transform.position.x) - HitWallThreshold;
                RightWall = hit1.collider.gameObject;
            }
            else
            {
                RightWallDis = Mathf.Abs(hit2.point.x - transform.position.x) - HitWallThreshold;
                RightWall = hit2.collider.gameObject;
            }
        }
        else if (hit1)
        {
            RightWallDis = Mathf.Abs(hit1.point.x - transform.position.x) - HitWallThreshold;
            RightWall = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            RightWallDis = Mathf.Abs(hit2.point.x - transform.position.x) - HitWallThreshold;
            RightWall = hit2.collider.gameObject;
        }
        else
        {
            RightWallDis = System.Int32.MaxValue;
            RightWall = null;
        }
        if (RightWall != null && RightWall.CompareTag("Platform_Totem"))
        {
            RightWallDis += PlatformSpeed.x * Time.deltaTime;
        }
    }

    private void CheckRightWallHitting()
    {
        float Dis = CheckOffset;
        if (RightWall != null && RightWall.CompareTag("Platform_Totem"))
        {
            Dis += PlatformSpeed.x * Time.deltaTime;
        }
        if (RightWallDis <= Dis)
        {
            HitRightWall = true;
        }
        else
        {
            HitRightWall = false;
        }
    }

    private bool IsMainCharacterDashing()
    {
        return gameObject == Character_Manager.Main_Character && GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Dashing;
    }

    private bool IsMainCharacterOverDashing()
    {
        return gameObject == Character_Manager.Main_Character && GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.OverDash;
    }

    private bool IsCharacterAimed()
    {
        return gameObject == Character_Manager.Main_Character && GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Aimed ||
            gameObject == Character_Manager.Fairy && GetComponent<Fairy_Status_Manager>().status == FairyStatus.Aimed;
    }

    private bool IsFairyIgnorePlatformSpeed()
    {
        return gameObject == Character_Manager.Fairy && (GetComponent<Fairy_Status_Manager>().status == FairyStatus.FloatPlatform ||
            GetComponent<Fairy_Status_Manager>().status == FairyStatus.Float);
    }

    private bool AbleToRectifyPos()
    {
        if(PlatformSpeed.y != 0)
        {
            return false;
        }
        if (PlatformSpeed.x != 0)
        {
            return false;
        }
        if (gameObject == Character_Manager.Main_Character)
        {
            if (GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Normal || GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Transporting)
            {
                return true;
            }
        }
        else if (gameObject == Character_Manager.Fairy)
        {
            if(GetComponent<Fairy_Status_Manager>().status==FairyStatus.Normal || GetComponent<Fairy_Status_Manager>().status == FairyStatus.Aiming)
            {
                return true;
            }
        }
        return false;
    }



    private void OnConnectedPlatformMoved(ConnectedPlatformMoved C)
    {
        if (C.Platform == ConnectedMovingPlatform)
        {
            ConnectedPlatformMoveFrameCount = C.FrameCount;
        }
    }

    private void RectifyPos()
    {
        if (AbleToRectifyPos())
        {
            if (LeftWallDis < 0)
            {
                transform.position += Vector3.left * LeftWallDis;
            }
            if (RightWallDis < 0)
            {
                transform.position += Vector3.right * RightWallDis;
            }
            if (GroundDis < 0 || (GroundDis>0 &&GroundDis<=CheckOffset))
            {
                transform.position -= Vector3.up * GroundDis;
            }
        }
    }
}
