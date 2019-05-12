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

    public Vector2 BodyOffset;

    public GameObject ConnectedMovingPlatform;
    private int layermask;


    public float OnGroundThreshold;
    public float OnGroundDetectOffset;

    public float HitWallThreshold;
    public float HitWallTopDetectOffset;
    public float HitWallDownDetectOffset;


    public float HitTopThreshold;
    public float HitTopDetectOffset;

    private const float DetectDis = 2;
    private const float CheckOffset = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Fairy") | 1 << LayerMask.NameToLayer("Path") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Portal") | 1<<LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Path")| 1<<LayerMask.NameToLayer("Slash");
        layermask = ~layermask;
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
        EventManager.instance.AddHandler<CharacterHitSpineEdge>(OnHitSpineEdge);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
        EventManager.instance.RemoveHandler<CharacterHitSpineEdge>(OnHitSpineEdge);
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
        if (gameObject.CompareTag("Main_Character"))
        {
            MainCharacterStatus status = GetComponent<Main_Character_Status_Manager>().status;
            if ((status == MainCharacterStatus.Normal || status == MainCharacterStatus.KnockBack) && !OnGround)
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
            if ((status == FairyStatus.Normal || status == FairyStatus.KnockBack) && !OnGround)
            {
                Gravity = GetComponent<Gravity_Data>().normal_gravityScale;
            }
            else
            {
                Gravity = 0;
            }
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
            if (!OnGround)
            {
                PlatformSpeed = Vector2.zero;
            }
            else
            {
                if (ConnectedMovingPlatform != null)
                {
                    PlatformSpeed = ConnectedMovingPlatform.GetComponent<Platform_Tolem>().CurrentSpeed;
                }
            }
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

        if (temp.y > 0 && temp.y * Time.deltaTime > TopDis)
        {
            temp.y = TopDis / Time.deltaTime;
            DashSpeed.y = 0;
            if (IsMainCharacterDashing())
            {
                GetComponent<Main_Character_Status_Manager>().status = MainCharacterStatus.OverDash;
            }
        }

        if (!IsMainCharacterDashing())
        {
            if (temp.y < 0 && temp.y * Time.deltaTime < -GroundDis)
            {
                temp.y = -GroundDis / Time.deltaTime;
                if (!IsMainCharacterDashing())
                {
                    DashSpeed.y = 0;
                }
            }

            if (temp.x > 0 && temp.x * Time.deltaTime > RightWallDis)
            {
                temp.x = RightWallDis / Time.deltaTime;
                if (!IsMainCharacterDashing())
                {
                    DashSpeed.x = 0;
                }
            }

            if (temp.x < 0 && temp.x * Time.deltaTime < -LeftWallDis)
            {
                temp.x = -LeftWallDis / Time.deltaTime;
                if (!IsMainCharacterDashing())
                {
                    DashSpeed.x = 0;
                }
            }
        }

        transform.position += (Vector3)temp* Time.deltaTime;
    }

    public void CheckGroundDis()
    {

        Vector3 OriPoint = transform.position + (Vector3)BodyOffset;
        RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector3.right * OnGroundDetectOffset, Vector2.down, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector3.left * OnGroundDetectOffset, Vector2.down, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.y - OriPoint.y) < Mathf.Abs(hit2.point.y - OriPoint.y))
            {
                GroundDis = Mathf.Abs(hit1.point.y - OriPoint.y) - OnGroundThreshold;
                Ground = hit1.collider.gameObject;
            }
            else
            {
                GroundDis = Mathf.Abs(hit2.point.y - OriPoint.y) - OnGroundThreshold;
                Ground = hit2.collider.gameObject;
            }
        }
        else if (hit1)
        {
            GroundDis = Mathf.Abs(hit1.point.y - OriPoint.y) - OnGroundThreshold;
            Ground = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            GroundDis = Mathf.Abs(hit2.point.y - OriPoint.y) - OnGroundThreshold;
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
        Vector3 OriPoint = transform.position + (Vector3)BodyOffset;
        RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector3.right * HitTopDetectOffset, Vector2.up, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector3.left * HitTopDetectOffset, Vector2.up, DetectDis, layermask);
        
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.y - OriPoint.y) < Mathf.Abs(hit2.point.y - OriPoint.y))
            {
                TopDis = Mathf.Abs(hit1.point.y - OriPoint.y) - HitTopThreshold;
                Top = hit1.collider.gameObject;
            }
            else
            {
                TopDis = Mathf.Abs(hit2.point.y - OriPoint.y) - HitTopThreshold;
                Top = hit2.collider.gameObject;
            }
            
        }
        else if (hit1)
        {
            TopDis = Mathf.Abs(hit1.point.y - OriPoint.y) - HitTopThreshold;
            Top = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            TopDis = Mathf.Abs(hit2.point.y - OriPoint.y) - HitTopThreshold;
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
        Vector3 OriPoint = transform.position + (Vector3)BodyOffset;
        RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector3.up * HitWallTopDetectOffset, Vector2.left, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector3.down * HitWallDownDetectOffset, Vector2.left, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.x - OriPoint.x) < Mathf.Abs(hit2.point.x - OriPoint.x))
            {
                LeftWallDis = Mathf.Abs(hit1.point.x - OriPoint.x) - HitWallThreshold;
                LeftWall = hit1.collider.gameObject;
            }
            else
            {
                LeftWallDis = Mathf.Abs(hit2.point.x - OriPoint.x) - HitWallThreshold;
                LeftWall = hit2.collider.gameObject;
            }
        }
        else if (hit1)
        {
            LeftWallDis = Mathf.Abs(hit1.point.x - OriPoint.x) - HitWallThreshold;
            LeftWall = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            LeftWallDis = Mathf.Abs(hit2.point.x - OriPoint.x) - HitWallThreshold;
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
        Vector3 OriPoint = transform.position + (Vector3)BodyOffset;
        RaycastHit2D hit1 = Physics2D.Raycast(OriPoint + Vector3.up * HitWallTopDetectOffset, Vector2.right, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(OriPoint + Vector3.down * HitWallDownDetectOffset, Vector2.right, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if(Mathf.Abs(hit1.point.x - OriPoint.x)< Mathf.Abs(hit2.point.x - OriPoint.x))
            {
                RightWallDis = Mathf.Abs(hit1.point.x - OriPoint.x) - HitWallThreshold;
                RightWall = hit1.collider.gameObject;
            }
            else
            {
                RightWallDis = Mathf.Abs(hit2.point.x - OriPoint.x) - HitWallThreshold;
                RightWall = hit2.collider.gameObject;
            }
        }
        else if (hit1)
        {
            RightWallDis = Mathf.Abs(hit1.point.x - OriPoint.x) - HitWallThreshold;
            RightWall = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            RightWallDis = Mathf.Abs(hit2.point.x - OriPoint.x) - HitWallThreshold;
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
            if (GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Normal)
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

    private void OnLoadLevel(LoadLevel L)
    {
        ConnectedMovingPlatform = null;
        speed = Vector2.zero;
        DashSpeed = Vector2.zero;
        PlatformSpeed = Vector2.zero;
    }

    private void OnHitSpineEdge(CharacterHitSpineEdge C)
    {
        if (C.Character == gameObject)
        {
            speed.x = GetComponent<KnockBack>().KnockBackSpeed.x * C.Spine.GetComponent<KnockBackSpine>().KnockBackDirection.x;
            speed.y = GetComponent<KnockBack>().KnockBackSpeed.y * C.Spine.GetComponent<KnockBackSpine>().KnockBackDirection.y;
        }
    }
}
