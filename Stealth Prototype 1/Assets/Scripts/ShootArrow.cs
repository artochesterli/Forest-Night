using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ShootArrow : MonoBehaviour
{
    public GameObject Connected_Arrow;
    public float ChargingTime;

    private Player player;
    private Vector2 direction;
    private List<GameObject> Aim_Line;
    private float ChargingTimeCount;
    private bool Charging;

    private const float Velocity_Charge_Speed = 10;
    private const float Aim_offset = 1;
    private const float mirroBounceStartPointOffset = 0.05f;
    private const float AimLineUnitPerMeter = 2;

    private const float AimDirectionSlowRotationSpeed = 2;
    private const float AimDirectionFastRotationSpeed = 60;
    
    private const float AimDirectionSlowChangeThreshold = 0.2f;
    private const float AimDirectionFastChangeThreshold = 1f;

    private const float AimRotationLimit = 150;

    private const float mirrorTopDownOffset = 0.05f;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
        Aim_Line = new List<GameObject>();
        direction = transform.right;
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
        var status = GetComponent<Fairy_Status_Manager>();
        if (status.status != FairyStatus.Transporting&&status.status!=FairyStatus.Aimed && status.status!=FairyStatus.KnockBack)
        {
            Check_Input();
        }
        CheckIfAimed();
    }

    private void Check_Input()
    {
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (Fairy_Status.status==FairyStatus.Normal && GetComponent<CharacterMove>().OnGround && player.GetButton("RT"))
        {
            Fairy_Status.status = FairyStatus.Aiming;
            if (Connected_Arrow == null)
            {
                Charging = true;
                ChargingTimeCount = 0;
                Connected_Arrow = (GameObject)Instantiate(Resources.Load("Prefabs/Arrow"));
                Connected_Arrow.transform.parent = transform;
                if (Vector2.Dot(transform.right, direction) < 0)
                {
                    direction.x = -direction.x;
                }
                Connected_Arrow.transform.position = transform.position + (Vector3)direction * Aim_offset;
                Connected_Arrow.transform.localScale = Vector3.zero;
                ClearAimLine();
                CreateAimLIne(direction, Connected_Arrow.transform.position);
            }
        }

        if (Fairy_Status.status == FairyStatus.Aiming)
        {
            if (!GetComponent<CharacterMove>().OnGround)
            {
                Fairy_Status.status = FairyStatus.Normal;
                return;
            }

            if (Charging)
            {
                if (ChargingTimeCount > ChargingTime)
                {
                    Connected_Arrow.transform.localScale = Vector3.one;
                    Connected_Arrow.GetComponent<Renderer>().material = Resources.Load("Material/Additive", typeof(Material)) as Material;
                    Charging = false;
                }
                Connected_Arrow.transform.localScale = Vector3.one * ChargingTimeCount / ChargingTime;
                ChargingTimeCount += Time.deltaTime;
            }

            float RightStickX = player.GetAxis("Right Stick X");
            if (Mathf.Abs(RightStickX) >= AimDirectionFastChangeThreshold)
            {
                if (RightStickX > 0)
                {
                    direction = Utility.instance.Rotate(direction, -AimDirectionFastRotationSpeed * Time.deltaTime);
                    
                }
                else
                {
                    direction = Utility.instance.Rotate(direction, AimDirectionFastRotationSpeed * Time.deltaTime);
                }
            }
            else if (Mathf.Abs(RightStickX) >= AimDirectionSlowChangeThreshold)
            {
                if (RightStickX > 0)
                {
                    direction = Utility.instance.Rotate(direction, -AimDirectionSlowRotationSpeed * Time.deltaTime);
                }
                else
                {
                    direction = Utility.instance.Rotate(direction, AimDirectionSlowRotationSpeed * Time.deltaTime);
                }
            }
            if (Vector2.Angle(Vector2.up, direction) > AimRotationLimit)
            {
                if (direction.x > 0)
                {
                    direction = Utility.instance.Rotate(Vector2.up, -AimRotationLimit);
                }
                else
                {
                    direction = Utility.instance.Rotate(Vector2.up, AimRotationLimit);
                }
                
            }

            direction.Normalize();

            ChangeFairyDirection();

            ClearAimLine();
            CreateAimLIne(direction, Connected_Arrow.transform.position);
            Connected_Arrow.transform.position = transform.position + (Vector3)direction * Aim_offset;

            if (player.GetButtonUp("RT"))
            {
                ClearAimLine();
                if (!Charging)
                {
                    Connected_Arrow.GetComponent<Arrow>().direction = direction;
                    Connected_Arrow.GetComponent<Arrow>().emit = true;
                    Connected_Arrow.transform.parent = null;
                    Connected_Arrow = null;
                }
                else
                {
                    Destroy(Connected_Arrow);
                }
                Fairy_Status.status = FairyStatus.Normal;
            }
        }

    }

    private void CreateAimLIne(Vector2 direction, Vector2 StartPoint)
    {
        int layermask = 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Portal") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1<<LayerMask.NameToLayer("Path") | 1<<LayerMask.NameToLayer("Main_Character");
        layermask = ~layermask;
        float mag = 100;
        RaycastHit2D hit= Physics2D.Raycast(StartPoint, direction, mag, layermask);
        int num = Mathf.FloorToInt((hit.point - StartPoint).magnitude*AimLineUnitPerMeter)+1;
        for(int i = 0; i < num; i++)
        {
            GameObject unit = (GameObject)Instantiate(Resources.Load("Prefabs/AimLineUnit"), StartPoint + direction * (1.0f/AimLineUnitPerMeter)*i, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.right ,direction)));
            Aim_Line.Add(unit);
        }
        if (hit.collider.gameObject.CompareTag("Mirror"))
        {
            float MirrorTopY = hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider2D>().size.y * hit.collider.gameObject.transform.localScale.y / 2;
            if (hit.point.y < MirrorTopY-mirrorTopDownOffset)
            {
                if (direction.x > 0)
                {
                    StartPoint = hit.point + Vector2.left * mirroBounceStartPointOffset;
                }
                else
                {
                    StartPoint = hit.point + Vector2.right * mirroBounceStartPointOffset;
                }
                direction.x = -direction.x;
                CreateAimLIne(direction, StartPoint);
            }
        }

    }

    private void ClearAimLine()
    {
        for (int i = 0; i < Aim_Line.Count; i++)
        {
            Destroy(Aim_Line[i]);
        }
    }

    private void ChangeFairyDirection()
    {
        if (direction.x > 0)
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

    private void CheckIfAimed()
    {
        if (GetComponent<Fairy_Status_Manager>().status == FairyStatus.Aimed)
        {
            Destroy(Connected_Arrow);
            ClearAimLine();
        }
    }

    private void OnLoadLevel(LoadLevel L)
    {
        ChargingTimeCount = 0;
        Destroy(Connected_Arrow);
    }
}
