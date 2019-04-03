using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Check : MonoBehaviour
{

    public float Alert_Time;
    public float Stunned_Time;
    public float Shoot_Time;
    public bool Attention_Drawn_Right;

    public float Alert_Distance;
    public float Aim_Distance;
    public float Shoot_Distance;
    public float time_count;
    public float alert_time_count;

    private GameObject detected_character;
    private Vector2 detected_character_hit_point;
    private List<GameObject> LaserLines;
    private GameObject hit_enemy;
    private float LaserLine_disappear_time_count;
    private bool laser_hit_mirror;
    private bool laser_not_hit_character;

    private const float RaycastAngle=60;
    private const float RaycastLines = 10;
    private const float LaserLine_disappear_time = 0.2f;
    private const float mirroBounceStartPointOffset = 0.01f;
    private const float InitialLaserOffset=0.6f;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CharacterDied>(OnCharacterDied);
        detected_character = null;
        time_count = 0;
        LaserLines = new List<GameObject>();
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

    }

    private void Stunned()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.status == EnemyStatus.Stunned)
        {
            time_count += Time.deltaTime;
            if (time_count > Stunned_Time)
            {
                time_count = 0;
                Enemy_Status.status = EnemyStatus.Patrol;
            }
        }
    }


    

    private void Find_Character()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        int layermask = 1 << LayerMask.NameToLayer("TutorialTrigger")| 1<<LayerMask.NameToLayer("Enemy")| 1<<LayerMask.NameToLayer("Invisible_Object") | 1<<LayerMask.NameToLayer("Arrow") | 1<<LayerMask.NameToLayer("Portal") | 1<< LayerMask.NameToLayer("PlatformTotemTrigger");
        layermask = ~layermask;
        float angle = -RaycastAngle / 2;
        float Interval = RaycastAngle / (RaycastLines - 1);
        for(int i = 0; i < RaycastLines; i++)
        {
            Vector2 direction= Utility.instance.Rotate((Vector2)transform.right, angle);
            angle += Interval;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, Alert_Distance, layermask);
            
            if (hit&&(hit.collider.gameObject.CompareTag("Main_Character") || hit.collider.gameObject.CompareTag("Fairy")))
            {
                GameObject hit_collider = hit.collider.gameObject;
                if (!hit_collider.GetComponent<Invisible>().invisible)
                {
                    if (Enemy_Status.status != EnemyStatus.ShootCharacter)
                    {
                        if (((Vector2)(hit_collider.transform.position - transform.position)).magnitude <= Shoot_Distance)
                        {
                            Enemy_Status.status = EnemyStatus.ShootCharacter;
                            time_count = 0;
                            detected_character = hit.collider.gameObject;
                            detected_character_hit_point=hit.point-(Vector2)detected_character.transform.position;
                            laser_not_hit_character = false;
                            laser_hit_mirror = false;
                            hit_enemy = null;
                        }
                        else
                        {
                            detected_character = hit.collider.gameObject;
                            detected_character_hit_point = hit.point - (Vector2)detected_character.transform.position;
                            Enemy_Status.status = EnemyStatus.Alert;
                        }
                        
                    }
                    return;
                }
            }
        }
        if (Enemy_Status.status == EnemyStatus.Alert)
        {
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
                time_count = 0;
                laser_not_hit_character = false;
                laser_hit_mirror=true;
                hit_enemy = null;
            }
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

            if (detected_character == null)
            {
                Enemy_Status.status = EnemyStatus.AlertRelease;
                return;
            }

            if (laser_not_hit_character)
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
            else
            {
                ClearLaserLine();
                Vector2 StartPoint = transform.position + transform.right * InitialLaserOffset;
                Vector2 direction = ((Vector2)detected_character.transform.position- StartPoint).normalized;
                GenerateLaserLine(direction, StartPoint);
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

    private void GenerateLaserLine(Vector2 direction, Vector2 StartPoint)
    {
        if (detected_character == null)
        {
            var Enemy_Status = GetComponent<Enemy_Status_Manager>();
            Enemy_Status.status = EnemyStatus.AlertRelease;
            return;
        }

        int layermask = 1 << LayerMask.NameToLayer("Bullet") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Portal") | 1<<LayerMask.NameToLayer("PlatformTotemTrigger");
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

            laser_not_hit_character = true;
            LaserLines.Add((GameObject)Instantiate(Resources.Load("Prefabs/LaserLine")));
            LaserLines[LaserLines.Count - 1].transform.localScale = new Vector3(dis, 1, 1);
            LaserLines[LaserLines.Count - 1].transform.position = (Vector3)StartPoint + (Vector3)(hit.point - StartPoint) / 2;
            LaserLines[LaserLines.Count - 1].transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);
            if (hit.collider.gameObject.CompareTag("Mirror"))
            {
                
                if (hit.point.y < hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider2D>().size.y / 2)
                {
                    laser_hit_mirror = true;
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
                if (hit.collider.gameObject.CompareTag("Enemy") && laser_hit_mirror)
                {
                    hit_enemy = hit.collider.gameObject;
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
            laser_not_hit_character = false;
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
            detected_character = null;
        }
    }
}
