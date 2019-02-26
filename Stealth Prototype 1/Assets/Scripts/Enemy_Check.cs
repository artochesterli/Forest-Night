using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Check : MonoBehaviour
{

    public float Alert_Time;
    public float Attention_Drawn_Time;
    public float Shoot_Time;
    public bool Attention_Drawn_Right;

    public float Alert_Distance;
    public float Aim_Distance;
    public float Shoot_Distance;
    public float time_count;
    public float alert_time_count;

    private GameObject detected_character;
    
    private const float RaycastAngle=60;
    private const float RaycastLines = 7;

    
    // Start is called before the first frame update
    void Start()
    {
        detected_character = null;
        time_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Find_Character();
        Attention_Drawn();
        Alert();
        Alert_Release();

    }

    private void Attention_Drawn()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.Status == Enemy_Status.ATTENTION_DRAWN)
        {
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
                Enemy_Status.Status = Enemy_Status.PATROL;
            }
        }
    }

    

    private void Find_Character()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        int layermask = 1 << LayerMask.NameToLayer("Bullet")| 1<<LayerMask.NameToLayer("Enemy")| 1<<LayerMask.NameToLayer("Invisible_Object");
  
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
                    if (Enemy_Status.Status != Enemy_Status.SHOOT_CHARACTER)
                    {
                        if (((Vector2)(hit_collider.transform.position - transform.position)).magnitude <= Shoot_Distance)
                        {
                            Enemy_Status.Status = Enemy_Status.SHOOT_CHARACTER;
                            time_count = 0;
                            detected_character = hit.collider.gameObject;
                            StartCoroutine(Shoot());
                        }
                        else
                        {
                            detected_character = hit.collider.gameObject;
                            Enemy_Status.Status = Enemy_Status.ALERT;
                        }
                        
                    }
                    return;
                }
            }
        }
        if (Enemy_Status.Status == Enemy_Status.ALERT)
        {
            Enemy_Status.Status = Enemy_Status.ALERT_RELEASE;
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
        if (Enemy_Status.Status==Enemy_Status.ALERT)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - alert_time_count / Alert_Time, 1 - alert_time_count / Alert_Time);
            alert_time_count += Time.deltaTime;
            if (alert_time_count > Alert_Time)
            {
                alert_time_count = Alert_Time;
                Enemy_Status.Status = Enemy_Status.SHOOT_CHARACTER;
                time_count = 0;
                StartCoroutine(Shoot());
            }
        }
    }

    private void Alert_Release()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        GameObject Indicator = transform.Find("Indicator").gameObject;
        if (Enemy_Status.Status == Enemy_Status.ALERT_RELEASE)
        {
            Indicator.GetComponent<SpriteRenderer>().enabled = true;
            Indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/exclamation_mark", typeof(Sprite)) as Sprite;
            Indicator.GetComponent<SpriteRenderer>().color = new Color(1, 1 - alert_time_count / Alert_Time, 1 - alert_time_count / Alert_Time);
            alert_time_count -= Time.deltaTime;
            if (alert_time_count < 0)
            {
                alert_time_count = 0;
                Enemy_Status.Status = Enemy_Status.PATROL;
            }
            detected_character = null;
        }
    }

    private IEnumerator Shoot()
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
    }

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
