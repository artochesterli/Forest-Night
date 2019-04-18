using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserState
{
    Null,
    HitCharacter,
    HitMirror,
    HitOther
}

public class Enemy_Check : MonoBehaviour
{

    public float Alert_Time;
    public float Stunned_Time;

    public float Alert_Distance;
    public float Shoot_Distance;
    public float Shoot_Star_Distance;
    public float stun_time_count;
    public float shoot_star_time_count;
    public float alert_time_count;

    private GameObject detected_star;
    private GameObject detected_character;
    private List<GameObject> LaserLines = new List<GameObject>();
    private GameObject hit_enemy;
    private float LaserLine_disappear_time_count;
    private LaserState CurrentLaserState;

    private const float RaycastAngle = 60;
    private const float RaycastLines = 60;
    private const float ShootStartTime = 0.1f;
    private const float LaserLine_disappear_time = 0.2f;
    private const float mirroBounceStartPointOffset = 0.01f;
    private const float InitialLaserOffset=0.4f;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CharacterDied>(OnCharacterDied);
        detected_character = null;
        detected_star = null;
        CurrentLaserState = LaserState.Null;
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CharacterDied>(OnCharacterDied);
        ClearLaserLine();
    }

    // Update is called once per frame
    void Update()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if(Enemy_Status.status != EnemyStatus.Stunned)
        {
            Find_Character();
        }
        
        Stunned();
        Alert();
        Alert_Release();
        Shoot_Character();
        Shoot_Star();

    }

    private void Stunned()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.status == EnemyStatus.Stunned)
        {
            stun_time_count += Time.deltaTime;
            if (stun_time_count > Stunned_Time)
            {
                stun_time_count = 0;
                Enemy_Status.status = EnemyStatus.Patrol;
            }
        }
    }

    private void Find_Character()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        int layermask = 1 << LayerMask.NameToLayer("TutorialTrigger")| 1<<LayerMask.NameToLayer("Enemy")| 1<<LayerMask.NameToLayer("Invisible_Object") | 1<<LayerMask.NameToLayer("Portal") | 1<< LayerMask.NameToLayer("PlatformTotemTrigger");
        if (Enemy_Status.status == EnemyStatus.ShootCharacter)
        {
            if (detected_character.CompareTag("Main_Character"))
            {
                layermask = layermask | 1 << LayerMask.NameToLayer("Fairy");
            }
            else if (detected_character.CompareTag("Fairy"))
            {
                layermask = layermask | 1 << LayerMask.NameToLayer("Main_Character");
            }
        }

        layermask = ~layermask;
        float angle = -RaycastAngle / 2;
        float Interval = RaycastAngle / (RaycastLines - 1);

        bool DetectedCharacterInSight = false;
        bool DetectAnyCharacter = false;

        for(int i = 0; i < RaycastLines; i++)
        {
            
            Vector2 direction= Utility.instance.Rotate((Vector2)transform.right, angle);
            angle += Interval;
            Vector2 Offset = transform.Find("View").localPosition;
            if (transform.right.x < 0)
            {
                Offset.x = -Offset.x;
            }
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Offset, direction, Alert_Distance, layermask);

            if (hit&&(hit.collider.gameObject.CompareTag("Main_Character") || hit.collider.gameObject.CompareTag("Fairy")))
            {
                DetectAnyCharacter = true;
                GameObject CurrentDetectedCharacter = hit.collider.gameObject;
                if (!CurrentDetectedCharacter.GetComponent<Invisible>().invisible)
                {
                    if (Enemy_Status.status != EnemyStatus.ShootCharacter)
                    {
                        if (detected_character==null || (CurrentDetectedCharacter.transform.position - transform.position).magnitude < (detected_character.transform.position - transform.position).magnitude)
                        {
                            detected_character = CurrentDetectedCharacter;
                        }

                        if (((Vector2)(CurrentDetectedCharacter.transform.position - transform.position)).magnitude <= Shoot_Distance)
                        {
                            DetectedCharacterInSight = true;
                            Enemy_Status.status = EnemyStatus.ShootCharacter;
                            CurrentLaserState = LaserState.Null;
                            hit_enemy = null;
                        }
                        else
                        {
                            Enemy_Status.status = EnemyStatus.Alert;
                        }
                    }
                    else
                    {
                        if (detected_character.CompareTag(CurrentDetectedCharacter.tag))
                        {
                            DetectedCharacterInSight = true;
                        }
                    }
                }
            }
            if(hit && hit.collider.gameObject.CompareTag("Arrow"))
            {
                if (Enemy_Status.status != EnemyStatus.ShootCharacter)
                {
                    GameObject hit_collider = hit.collider.gameObject;
                    if (((Vector2)(hit_collider.transform.position - transform.position)).magnitude <= Shoot_Star_Distance)
                    {
                        Enemy_Status.status = EnemyStatus.ShootStar;
                        detected_star = hit_collider;
                        detected_star.GetComponent<Arrow>().Aimed = true;
                    }
                    return;
                }
                
            }

        }
        if (Enemy_Status.status == EnemyStatus.ShootCharacter)
        {
            if (!DetectedCharacterInSight)
            {
                Vector2 Offset = transform.Find("View").localPosition;
                if (transform.right.x < 0)
                {
                    Offset.x = -Offset.x;
                }
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Offset, (detected_character.transform.position-transform.position).normalized, Alert_Distance, layermask);
                if(!(hit && hit.collider.gameObject.CompareTag("Mirror")))
                {
                    detected_character = null;
                    Enemy_Status.status = EnemyStatus.AlertRelease;
                }
                
            }
        }

        if (Enemy_Status.status == EnemyStatus.Alert && !DetectAnyCharacter)
        {
            detected_character = null;
            Enemy_Status.status = EnemyStatus.AlertRelease;
        }

    }

    private void Alert ()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.status==EnemyStatus.Alert)
        {
            alert_time_count += Time.deltaTime;
            if (alert_time_count > Alert_Time)
            {
                alert_time_count = Alert_Time;
                Enemy_Status.status = EnemyStatus.ShootCharacter;
                CurrentLaserState = LaserState.Null;
                hit_enemy = null;
            }
        }
    }

    private void Shoot_Star()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.status == EnemyStatus.ShootStar)
        {
            if (shoot_star_time_count > ShootStartTime)
            {
                shoot_star_time_count = 0;
                Enemy_Status.status = EnemyStatus.AlertRelease;
                Destroy(detected_star);
                return;
            }
            shoot_star_time_count += Time.deltaTime;
            ClearLaserLine();
            LaserLineShootStar(transform.position + transform.right * InitialLaserOffset);
        }
    }

    private void Shoot_Character()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        GameObject Indicator = transform.Find("Indicator").gameObject;
        if (Enemy_Status.status == EnemyStatus.ShootCharacter)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            alert_time_count = Alert_Time;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - alert_time_count / Alert_Time, 1 - alert_time_count / Alert_Time);

            if(CurrentLaserState==LaserState.Null || CurrentLaserState == LaserState.HitOther || CurrentLaserState == LaserState.HitCharacter)
            {
                ClearLaserLine();
                Vector2 StartPoint = transform.position + transform.right * InitialLaserOffset;
                Vector2 direction = ((Vector2)detected_character.transform.position - StartPoint).normalized;
                GenerateLaserLine(direction, StartPoint);
            }
            else if(CurrentLaserState == LaserState.HitMirror)
            {
                if (LaserLine_disappear_time_count >= LaserLine_disappear_time)
                {
                    Enemy_Status.status = EnemyStatus.AlertRelease;
                    if (hit_enemy != null)
                    {
                        Destroy(hit_enemy);
                    }
                }
                LaserLine_disappear_time_count += Time.deltaTime;
            }

        }
        else
        {
            ClearLaserLine();
        }
    }

    private void ClearLaserLine()
    {
        for (int i = 0; i < LaserLines.Count; i++)
        {
            Destroy(LaserLines[i]);
        }
    }


    private void LaserLineShootStar(Vector2 StartPoint)
    {
        float dis = ((Vector2)detected_star.transform.position-StartPoint).magnitude;
        Vector2 direction = ((Vector2)detected_star.transform.position - StartPoint).normalized;
        LaserLines.Add((GameObject)Instantiate(Resources.Load("Prefabs/LaserLine")));
        LaserLines[LaserLines.Count - 1].transform.localScale = new Vector3(dis, 1, 1);
        LaserLines[LaserLines.Count - 1].transform.position = (Vector3)StartPoint + (Vector3)((Vector2)detected_star.transform.position - StartPoint) / 2;
        LaserLines[LaserLines.Count - 1].transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);

    }

    private void GenerateLaserLine(Vector2 direction, Vector2 StartPoint)
    {
        int layermask = 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Portal") | 1<<LayerMask.NameToLayer("PlatformTotemTrigger");
        if (CurrentLaserState == LaserState.HitCharacter)
        {
            if (detected_character.CompareTag("Fairy"))
            {
                layermask = layermask | 1 << LayerMask.NameToLayer("Main_Character");
            }
            else if (detected_character.CompareTag("Main_Character"))
            {
                layermask = layermask | 1 << LayerMask.NameToLayer("Fairy");
            }
        }
        layermask = ~layermask;

        float mag = 100;
        RaycastHit2D hit= Physics2D.Raycast(StartPoint, direction, mag, layermask);

        if (!hit.collider.gameObject.CompareTag(detected_character.tag))
        {
            GameObject ob = detected_character;
            if (ob.CompareTag("Fairy"))
            {
                var status = ob.GetComponent<Fairy_Status_Manager>();
                if (status.status == FairyStatus.Aimed)
                {
                    status.status = FairyStatus.Normal;
                }
            }
            else if (ob.CompareTag("Main_Character"))
            {
                var status = ob.GetComponent<Main_Character_Status_Manager>();
                if (status.status == MainCharacterStatus.Aimed)
                {
                    status.status = MainCharacterStatus.Normal;
                }
            }

            float dis = (hit.point - StartPoint).magnitude;

            LaserLines.Add((GameObject)Instantiate(Resources.Load("Prefabs/LaserLine")));
            LaserLines[LaserLines.Count - 1].transform.localScale = new Vector3(dis, 1, 1);
            LaserLines[LaserLines.Count - 1].transform.position = (Vector3)StartPoint + (Vector3)(hit.point - StartPoint) / 2;
            LaserLines[LaserLines.Count - 1].transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);
            if (hit.collider.gameObject.CompareTag("Mirror"))
            {
                if (hit.point.y < hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider2D>().size.y / 2)
                {
                    CurrentLaserState = LaserState.HitMirror;
                    if (direction.x > 0)
                    {
                        StartPoint = hit.point + Vector2.left * mirroBounceStartPointOffset;
                    }
                    else
                    {
                        StartPoint = hit.point + Vector2.right * mirroBounceStartPointOffset;
                    }

                    direction.x = -direction.x;
                    GenerateLaserLine(direction, StartPoint);
                }
            }
            else
            {
                if (CurrentLaserState == LaserState.HitMirror)
                {
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        hit_enemy = hit.collider.gameObject;
                    }
                }
                else
                {
                    CurrentLaserState = LaserState.HitOther;
                }
            }
        }
        else
        {
            GameObject ob = detected_character;
            if (ob.CompareTag("Fairy"))
            {
                var status = ob.GetComponent<Fairy_Status_Manager>();
                status.status = FairyStatus.Aimed;
            }
            else if (ob.CompareTag("Main_Character"))
            {
                var status = ob.GetComponent<Main_Character_Status_Manager>();
                status.status = MainCharacterStatus.Aimed;
            }
            float dis = (hit.point - StartPoint).magnitude;
            CurrentLaserState = LaserState.HitCharacter;
            LaserLines.Add((GameObject)Instantiate(Resources.Load("Prefabs/LaserLine")));
            LaserLine_disappear_time_count = 0;
            LaserLines[LaserLines.Count - 1].transform.localScale = new Vector3(dis, 1, 1);
            LaserLines[LaserLines.Count - 1].transform.position = (Vector3)StartPoint + (Vector3)(hit.point - StartPoint) / 2;
            LaserLines[LaserLines.Count - 1].transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);
        }
    }

    private void Alert_Release()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.status == EnemyStatus.AlertRelease)
        {
            alert_time_count -= Time.deltaTime;
            if (alert_time_count < 0)
            {
                alert_time_count = 0;
                Enemy_Status.status = EnemyStatus.Patrol;
            }
            detected_character = null;
        }
    }

    private void OnCharacterDied(CharacterDied C)
    {
        if (C.DeadCharacter == detected_character)
        {
            shoot_star_time_count = 0;
            alert_time_count = 0;
            stun_time_count = 0;
            detected_character = null;
            detected_star = null;
            hit_enemy = null;
            LaserLine_disappear_time_count = 0;
            CurrentLaserState = LaserState.Null;
            ClearLaserLine();
        }
    }
}
