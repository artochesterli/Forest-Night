using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Check : MonoBehaviour
{

    public float Check_Character_Bar;
    public float Check_Object_Time;
    public bool Check_Object_Right;
    public float check_object_time_count;

    private GameObject detected_character;
    
    private const float RaycastAngle=60;
    private const float RaycastLines = 7;
    private const float RaycastDis = 10;
    private const float irratate_dis = 3;
    private const float Check_Character_Bar_full=100;
    private const float Check_Character_Bar_Max_Up_Speed=300;
    private const float Check_Character_Bar_Fall_Speed = 30;

    
    // Start is called before the first frame update
    void Start()
    {
        detected_character = null;
        Check_Character_Bar = 0;
        check_object_time_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Find_Character();
        Check_Object();
        Check_Character();

    }

    private void Check_Object()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.Status == Enemy_Status.CHECK_OBJECT)
        {
            if (Check_Object_Right)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            check_object_time_count += Time.deltaTime;
            if (check_object_time_count > Check_Object_Time)
            {
                check_object_time_count = 0;
                Enemy_Status.Status = Enemy_Status.PATROL;
            }
        }
    }

    

    private void Find_Character()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        int layermask = 1 << LayerMask.NameToLayer("Invisible_Ward")| 1<<LayerMask.NameToLayer("Enemy")| 1<<LayerMask.NameToLayer("Invisible_Object");
  
        layermask = ~layermask;
        float angle = -RaycastAngle / 2;
        float Interval = RaycastAngle / (RaycastLines - 1);
        for(int i = 0; i < RaycastLines; i++)
        {
            Vector2 direction= Rotate((Vector2)transform.right, angle);
            angle += Interval;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, RaycastDis, layermask);
            if (hit)
            {
                //Debug.Log(hit.collider.gameObject.name);
            }
            if (hit&&(hit.collider.gameObject.CompareTag("Main_Character") || hit.collider.gameObject.CompareTag("Fairy")))
            {
                if (!hit.collider.gameObject.GetComponent<Invisible>().invisible)
                {
                    Enemy_Status.Status = Enemy_Status.CHECK_CHARACTER;
                    detected_character = hit.collider.gameObject;
                    return;
                }
            }
        }
        if(Enemy_Status.Status == Enemy_Status.CHECK_CHARACTER)
        {
            if (detected_character != null)
            {
                if (detected_character.transform.position.x > transform.position.x)
                {
                    Check_Object_Right = true;
                }
                else
                {
                    Check_Object_Right = false;
                }
                check_object_time_count = 0;
                Enemy_Status.Status = Enemy_Status.CHECK_OBJECT;
            }
            else
            {
                Enemy_Status.Status = Enemy_Status.PATROL;
            }
            
        }
        detected_character = null;
    }

    private void Check_Character()
    {
        var Enemy_Status = GetComponent<Enemy_Status_Manager>();
        if (Enemy_Status.Status==Enemy_Status.CHECK_CHARACTER)
        {
            float dis = ((Vector2)(detected_character.transform.position - transform.position)).magnitude;
            Check_Character_Bar += Check_Character_Bar_Max_Up_Speed / dis * Time.deltaTime;
            if (dis < irratate_dis)
            {
                Check_Character_Bar = Check_Character_Bar_full;
            }
            if (Check_Character_Bar >= Check_Character_Bar_full)
            {
                Destroy(detected_character);
            }
        }
        else
        {
            Check_Character_Bar -= Check_Character_Bar_Fall_Speed * Time.deltaTime;
            if (Check_Character_Bar < 0)
            {
                Check_Character_Bar = 0;
            }
        }
        GetComponent<SpriteRenderer>().color = new Color(1, 1 - Check_Character_Bar / Check_Character_Bar_full, 1 - Check_Character_Bar / Check_Character_Bar_full);
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
