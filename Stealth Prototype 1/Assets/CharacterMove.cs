using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public Vector2 speed;
    public Vector2 DashSpeed;
    public Vector2 PlatformSpeed;
    public float Gravity;

    public bool HitWall;
    public float WallDis;
    public Vector2 WallDirection;
    public GameObject Wall;

    public bool HitTop;
    public float TopDis;
    public GameObject Top;

    public bool OnGround;
    public float GroundDis;
    public GameObject Ground;

    public GameObject ConnectedMovingPlatform;
    private int ConnectedPlatformMoveFrameCount;

    private const float OnGroundThreshold = 0.5f;
    private const float OnGroundDetectOffset = 0.4f;

    private const float HitWallThreshold = 0.5f;
    private const float HitWallDetectOffset = 0.4f;


    private const float HitTopThreshold = 0.5f;
    private const float HitTopDetectOffset = 0.4f;

    private const float DetectDis = 2;
    private const float CheckOffset = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<ConnectedPlatformMoved>(OnConnectedPlatformMoved);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ConnectedPlatformMoved>(OnConnectedPlatformMoved);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundDis();
        CheckWallDis();
        CheckTopDis();
        CheckGroundHitting();
        CheckWallHitting();
        CheckTopHitting();
        CheckCollide();

        SetGravity();
        GravityEffect();
        SurfaceHittingSpeedChange();
        Move();
        RectifyPos();
    }

    private void CheckCollide()
    {
        if (HitWall && Wall.CompareTag("Spine") || OnGround&& Ground.CompareTag("Spine") || HitTop&&Top.CompareTag("Spine"))
        {
            EventManager.instance.Fire(new CharacterDied(gameObject));
            Destroy(gameObject);
        }
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

    private void SurfaceHittingSpeedChange()
    {
        if (!IsMainCharacterDashing())
        {
            if (HitWall)
            {
                speed.x = 0;
            }
            if (HitTop && speed.y > 0 || OnGround && speed.y < 0)
            {
                speed.y = 0;
            }
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
                if (Ground.CompareTag("Platform_Totem") || Ground.CompareTag("Totem_Platform"))
                {
                    temp.y += PlatformSpeed.y;
                }
                DashSpeed = Vector2.zero;
            }

            if (temp.y < 0 && GroundDis >= 0 && temp.y * Time.deltaTime < -GroundDis)
            {
                temp.y = -GroundDis / Time.deltaTime;
                if (Ground.CompareTag("Platform_Totem") || Ground.CompareTag("Totem_Platform"))
                {
                    temp.y += PlatformSpeed.y;
                }
                DashSpeed = Vector2.zero;
            }

            if (temp.x > 0)
            {
                if (temp.x * Time.deltaTime > WallDis && WallDis >= 0 && WallDirection.x > 0)
                {
                    temp.x = WallDis / Time.deltaTime;
                    if (Wall.CompareTag("Totem_Platform"))
                    {
                        temp.x += PlatformSpeed.x;
                    }
                    DashSpeed = Vector2.zero;
                }
            }
            else
            {
                if (temp.x * Time.deltaTime < -WallDis && WallDis >= 0 && WallDirection.x < 0)
                {
                    temp.x = -WallDis / Time.deltaTime;
                    if (Wall.CompareTag("Totem_Platform"))
                    {
                        temp.x += PlatformSpeed.x;
                    }
                    DashSpeed = Vector2.zero;
                }
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
        

        int layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Fairy") | 1 << LayerMask.NameToLayer("Path") | 1 << LayerMask.NameToLayer("Gem") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Portal");
        layermask = ~layermask;


        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.right * OnGroundDetectOffset, Vector2.down, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.left * OnGroundDetectOffset, Vector2.down, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.y - transform.position.y) < Mathf.Abs(hit2.point.y - transform.position.y))
            {
                GroundDis = Mathf.Abs(hit1.point.y - transform.position.y) - HitWallThreshold;
                Ground = hit1.collider.gameObject;
            }
            else
            {
                GroundDis = Mathf.Abs(hit2.point.y - transform.position.y) - HitWallThreshold;
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
        
    }

    private void CheckGroundHitting()
    {
        float Dis = CheckOffset;
        if (ConnectedMovingPlatform != null)
        {
            if (Time.frameCount == ConnectedPlatformMoveFrameCount)
            {
                Dis -= PlatformSpeed.y * Time.deltaTime;
            }
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
        int layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Fairy") | 1 << LayerMask.NameToLayer("Path") | 1 << LayerMask.NameToLayer("Gem") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Portal");
        layermask = ~layermask;

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.right * HitTopDetectOffset, Vector2.up, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.left * HitTopDetectOffset, Vector2.up, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if (Mathf.Abs(hit1.point.y - transform.position.y) < Mathf.Abs(hit2.point.y - transform.position.y))
            {
                TopDis = Mathf.Abs(hit1.point.y - transform.position.y) - HitWallThreshold;
                Top = hit1.collider.gameObject;
            }
            else
            {
                TopDis = Mathf.Abs(hit2.point.y - transform.position.y) - HitWallThreshold;
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

    public void CheckWallDis()
    {
        int layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Fairy") | 1 << LayerMask.NameToLayer("Path") | 1 << LayerMask.NameToLayer("Gem") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Portal");
        layermask = ~layermask;

        if (speed.x + DashSpeed.x + PlatformSpeed.x== 0)
        {
            WallDirection = transform.right;
            if (gameObject.CompareTag("Fairy") && GetComponent<Fairy_Status_Manager>().status == FairyStatus.Aiming)
            {
                WallDirection = -transform.right;
            }
        }
        else
        {
            if (speed.x + DashSpeed.x + PlatformSpeed.x > 0)
            {
                WallDirection = Vector2.right;
            }
            else
            {
                WallDirection = Vector2.left;
            }
        }

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.up * HitWallDetectOffset, WallDirection, DetectDis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.down * HitWallDetectOffset, WallDirection, DetectDis, layermask);
        if (hit1 && hit2)
        {
            if(Mathf.Abs(hit1.point.x - transform.position.x)< Mathf.Abs(hit2.point.x - transform.position.x))
            {
                WallDis = Mathf.Abs(hit1.point.x - transform.position.x) - HitWallThreshold;
                Wall = hit1.collider.gameObject;
            }
            else
            {
                WallDis = Mathf.Abs(hit2.point.x - transform.position.x) - HitWallThreshold;
                Wall = hit2.collider.gameObject;
            }
        }
        else if (hit1)
        {
            WallDis = Mathf.Abs(hit1.point.x - transform.position.x) - HitWallThreshold;
            Wall = hit1.collider.gameObject;
        }
        else if (hit2)
        {
            WallDis = Mathf.Abs(hit2.point.x - transform.position.x) - HitWallThreshold;
            Wall = hit2.collider.gameObject;
        }
        else
        {
            WallDis = System.Int32.MaxValue;
            Wall = null;
        }
    }

    private void CheckWallHitting()
    {
        if (WallDis <= CheckOffset)
        {
            HitWall = true;
        }
        else
        {
            HitWall = false;
        }
    }

    private bool IsMainCharacterDashing()
    {
        return gameObject == Character_Manager.Main_Character && GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Dashing;
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
        if (PlatformSpeed.y == 0)
        {
            if (GroundDis < 0 || GroundDis>0 &&GroundDis<=CheckOffset)
            {
                transform.position -= Vector3.up * GroundDis;
            }
        }
    }
}
