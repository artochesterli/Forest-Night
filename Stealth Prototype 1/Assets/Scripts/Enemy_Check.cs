using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Check : MonoBehaviour
{

    public float Alert_Time;
    public float Attention_Drawn_Time;
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
        detected_character = null;
        time_count = 0;
        LaserLines = new List<GameObject>();
    }

    private void OnDestroy()
    {
        for(int i = 0; i < LaserLines.Count; i++)
        {
            Destroy(LaserLines[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if(Enemy_Status.status != Enemy_Status.STUNNED)
        {
            Find_Character();
        }
        
        Stunned();
        //Attention_Drawn();
        Alert();
        Alert_Release();
        Shoot_Character();

    }

    private void Stunned()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.status == Enemy_Status.STUNNED)
        {
            GameObject Indicator = transform.Find("Indicator").gameObject;
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/Stunned Mark", typeof(Sprite)) as Sprite;
            Indicator.transform.rotation = new Quaternion(0, 0, 0, 0);
            time_count += Time.deltaTime;
            if (time_count > Stunned_Time)
            {
                time_count = 0;
                Enemy_Status.status = Enemy_Status.PATROL;
            }
        }
    }

    private void Attention_Drawn()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.status == Enemy_Status.ATTENTION_DRAWN)
        {
            GameObject Indicator = transform.Find("Indicator").gameObject;
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/Question Mark", typeof(Sprite)) as Sprite;
            Indicator.transform.rotation = new Quaternion(0, 0, 0, 0);
            if (Attention_Drawn_Right)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            time_count += Time.deltaTime;
            if (time_count > Attention_Drawn_Time)
            {
                time_count = 0;
                Enemy_Status.status = Enemy_Status.PATROL;
            }
        }
    }

    

    private void Find_Character()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        int layermask = 1 << LayerMask.NameToLayer("Bullet")| 1<<LayerMask.NameToLayer("Enemy")| 1<<LayerMask.NameToLayer("Invisible_Object") | 1<<LayerMask.NameToLayer("Arrow") | 1<<LayerMask.NameToLayer("Portal") | 1<< LayerMask.NameToLayer("PlatformTotemTrigger");
        layermask = ~layermask;
        float angle = -RaycastAngle / 2;
        float Interval = RaycastAngle / (RaycastLines - 1);
        for(int i = 0; i < RaycastLines; i++)
        {
            Vector2 direction= Rotate((Vector2)transform.right, angle);
            angle += Interval;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, Alert_Distance, layermask);
            
            if (hit&&(hit.collider.gameObject.CompareTag("Main_Character") || hit.collider.gameObject.CompareTag("Fairy")))
            {
                GameObject hit_collider = hit.collider.gameObject;
                if (!hit_collider.GetComponent<Invisible>().invisible)
                {
                    if (Enemy_Status.status != Enemy_Status.SHOOT_CHARACTER)
                    {
                        if (((Vector2)(hit_collider.transform.position - transform.position)).magnitude <= Shoot_Distance)
                        {
                            Enemy_Status.status = Enemy_Status.SHOOT_CHARACTER;
                            time_count = 0;
                            detected_character = hit.collider.gameObject;
                            detected_character_hit_point=hit.point-(Vector2)detected_character.transform.position;
                            laser_not_hit_character = false;
                            laser_hit_mirror = false;
                            hit_enemy = null;
                            //StartCoroutine(Shoot());
                        }
                        else
                        {
                            detected_character = hit.collider.gameObject;
                            detected_character_hit_point = hit.point - (Vector2)detected_character.transform.position;
                            Enemy_Status.status = Enemy_Status.ALERT;
                        }
                        
                    }
                    return;
                }
            }
        }
        if (Enemy_Status.status == Enemy_Status.ALERT)
        {
            Enemy_Status.status = Enemy_Status.ALERT_RELEASE;
        }
        
        /*if(Enemy_Status.Status == Enemy_Status.ALERT_RELEASE)
        {
            alert_time_count -= Time.deltaTime;
            if (alert_time_count < 0)
            {
                alert_time_count = 0;
                Enemy_Status.Status = Enemy_Status.PATROL;
            }
            detected_character = null;
        }*/
            

    }

    private void Alert ()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        GameObject Indicator = transform.Find("Indicator").gameObject;
        if (Enemy_Status.status==Enemy_Status.ALERT)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - alert_time_count / Alert_Time, 1 - alert_time_count / Alert_Time);
            alert_time_count += Time.deltaTime;
            if (alert_time_count > Alert_Time)
            {
                alert_time_count = Alert_Time;
                Enemy_Status.status = Enemy_Status.SHOOT_CHARACTER;
                time_count = 0;
                laser_not_hit_character = false;
                laser_hit_mirror=true;
                hit_enemy = null;
                //StartCoroutine(Shoot());
            }
        }
    }

    private void Shoot_Character()
    {
        
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        GameObject Indicator = transform.Find("Indicator").gameObject;
        if (Enemy_Status.status == Enemy_Status.SHOOT_CHARACTER)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            alert_time_count = Alert_Time;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - alert_time_count / Alert_Time, 1 - alert_time_count / Alert_Time);
            
            if (laser_not_hit_character)
            {
               
                if (LaserLine_disappear_time_count >= LaserLine_disappear_time)
                {
                    Enemy_Status.status = Enemy_Status.ALERT_RELEASE;
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
                if (detected_character != null)
                {
                    Vector2 StartPoint = transform.position + transform.right * InitialLaserOffset;
                    Vector2 direction = ((Vector2)detected_character.transform.position- StartPoint).normalized;
                    GenerateLaserLine(direction, StartPoint);
                }
                else
                {
                    Enemy_Status.status = Enemy_Status.ALERT_RELEASE;
                }
                //generate_laser_line();
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
            Enemy_Status.status = Enemy_Status.ALERT_RELEASE;
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
                if (status.status == status.AIMED)
                {
                    status.status = status.NORMAL;
                }
            }
            else if (ob.CompareTag("Main_Character"))
            {
                var status = ob.GetComponent<Main_Character_Status_Manager>();
                if (status.status == status.AIMED)
                {
                    status.status = status.NORMAL;
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
                laser_hit_mirror = true;
                StartPoint = hit.point - Vector2.one * direction.x * mirroBounceStartPointOffset;
                direction.x = -direction.x;
                GenerateLaserLine(direction ,StartPoint);
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
                status.status = status.AIMED;
            }
            else if (ob.CompareTag("Main_Character"))
            {
                var status = ob.GetComponent<Main_Character_Status_Manager>();
                status.status = status.AIMED;
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

    /*private void generate_laser_line()
    {
        for(int i = 0; i < LaserLines.Count; i++)
        {
            Destroy(LaserLines[i]);
        }
        if (detected_character == null)
        {
            var Enemy_Status = GetComponent<Enemy_Status_Manager>();
            Enemy_Status.status = Enemy_Status.ALERT_RELEASE;
            return;
        }

        Vector2 start_point = transform.position+transform.right*InitialLaserOffset;
        Vector2 direction = (Vector2)detected_character.transform.position - start_point;
        direction.Normalize();
        float mag = (detected_character.transform.position - transform.position).magnitude;

        int layermask = 1 << LayerMask.NameToLayer("Bullet") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Portal");
        layermask = ~layermask;

        bool MoreBounce = true;
        while (MoreBounce)
        {
            RaycastHit2D hit;
            if (!laser_not_hit_character)
            {
                hit = Physics2D.Raycast(start_point, direction, mag, layermask);
            }
            else
            {
                hit = Physics2D.Raycast(start_point, direction, 100, layermask);
            }
            if (!hit)
            {
                return;
            }
            if (!hit.collider.gameObject.CompareTag(detected_character.tag))
            {
                GameObject ob = detected_character;
                if (ob.CompareTag("Fairy"))
                {
                    var status = ob.GetComponent<Fairy_Status_Manager>();
                    if (status.status == status.AIMED)
                    {
                        status.status = status.NORMAL;
                    }
                }
                else if (ob.CompareTag("Main_Character"))
                {
                    var status = ob.GetComponent<Main_Character_Status_Manager>();
                    if (status.status == status.AIMED)
                    {
                        status.status = status.NORMAL;
                    }
                    
                }

                Vector2 hitpoint = hit.point;
                float dis = (hitpoint - start_point).magnitude;

                laser_not_hit_character = true;
                LaserLines.Add((GameObject)Instantiate(Resources.Load("Prefabs/LaserLine")));
                LaserLines[LaserLines.Count - 1].transform.localScale = new Vector3(dis, 1, 1);
                LaserLines[LaserLines.Count - 1].transform.position = (Vector3)start_point + (Vector3)(hitpoint - start_point) / 2;
                LaserLines[LaserLines.Count - 1].transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);
                if (hit.collider.gameObject.CompareTag("Mirror"))
                {
                    laser_hit_mirror = true;
                    MoreBounce = true;
                    start_point = hitpoint-Vector2.one*direction.x*mirroBounceStartPointOffset;
                    direction.x = -direction.x;
                }
                else
                {
                    MoreBounce = false;
                    if (hit.collider.gameObject.CompareTag("Enemy")&&laser_hit_mirror)
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
                    status.status = status.AIMED;
                }
                else if (ob.CompareTag("Main_Character"))
                {
                    var status = ob.GetComponent<Main_Character_Status_Manager>();
                    status.status = status.AIMED;
                }
                laser_not_hit_character = false;
                MoreBounce = false;
                LaserLines.Add((GameObject)Instantiate(Resources.Load("Prefabs/LaserLine")));
                LaserLine_disappear_time_count = 0;
                LaserLines[LaserLines.Count - 1].transform.localScale = new Vector3(mag, 1, 1);
                LaserLines[LaserLines.Count - 1].transform.position = transform.position + (detected_character.transform.position - transform.position) / 2;
                LaserLines[LaserLines.Count - 1].transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);
            }
        }

    }*/

    private void Alert_Release()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        GameObject Indicator = transform.Find("Indicator").gameObject;
        if (Enemy_Status.status == Enemy_Status.ALERT_RELEASE)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - alert_time_count / Alert_Time, 1 - alert_time_count / Alert_Time);
            alert_time_count -= Time.deltaTime;
            if (alert_time_count < 0)
            {
                alert_time_count = 0;
                Enemy_Status.status = Enemy_Status.PATROL;
            }
            detected_character = null;
        }
    }



    /*private IEnumerator Shoot()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        alert_time_count = Alert_Time;
        GameObject Indicator = transform.Find("Indicator").gameObject;
        Indicator.GetComponent<SpriteRenderer>().enabled = true;
        Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
        Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);

        Vector2 direction = detected_character.transform.position - transform.position;
        GameObject bullet = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet_Enemy"), transform.position, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<Bullet_Enemy>().target = detected_character;
        bullet.transform.localScale = Vector3.zero;
        bullet.transform.parent = transform;
        float time_count = 0;
        while (time_count < Shoot_Time)
        {
            bullet.transform.localScale = Vector3.one * time_count / Shoot_Time;
            time_count += Time.deltaTime;
            yield return null;
        }
        bullet.transform.parent = null;
        bullet.GetComponent<Bullet_Enemy>().StartCoroutine(bullet.GetComponent<Bullet_Enemy>().fly());
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        Enemy_Status.Status = Enemy_Status.ALERT_RELEASE;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }*/

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
