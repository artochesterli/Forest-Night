using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Check : MonoBehaviour
{
    public bool IsChecking;
    public bool IsCheckingCharacter;
    public float Check_Character_Bar;
    public float Check_Object_Time;
    public bool Check_Right;

    private GameObject detected_character;
    private const float RaycastAngle=60;
    private const float RaycastLines = 7;
    private const float RaycastDis = 8;
    private const float Check_Character_Bar_full=100;
    private const float Check_Character_Bar_Max_Up_Speed=300;
    private const float Check_Character_Bar_Fall_Speed = 30;
    // Start is called before the first frame update
    void Start()
    {
        detected_character = null;
        IsChecking = false;
        IsCheckingCharacter = false;
        Check_Character_Bar = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Find_Character();
        Check_Character();


        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Check_Object());
        }
    }

    public IEnumerator Check_Object()
    {
        if (IsCheckingCharacter)
        {
            yield break;
        }
        GetComponent<Enemy_Patrol>().IsPatrol = false;
        IsChecking = true;
        IsCheckingCharacter = false;
        if (Check_Right)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        float time_count = 0;
        while (time_count < Check_Object_Time)
        {
            if (IsCheckingCharacter)
            {
                yield break;
            }
            time_count += Time.deltaTime;
            yield return null;
        }
        GetComponent<Enemy_Patrol>().IsPatrol = true;
        IsChecking = false;

    }

    private void Find_Character()
    {
        int layermask = 1 << LayerMask.NameToLayer("Enemy");
        layermask = ~layermask;
        float angle = -RaycastAngle / 2;
        float Interval = RaycastAngle / (RaycastLines - 1);
        for(int i = 0; i < RaycastLines; i++)
        {
            Vector2 direction= Rotate((Vector2)transform.right, angle);
            angle += Interval;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, RaycastDis, layermask);
            if (hit&&(hit.collider.gameObject.CompareTag("Main_Character") || hit.collider.gameObject.CompareTag("Fairy")))
            {
                IsChecking = true;
                IsCheckingCharacter = true;
                detected_character = hit.collider.gameObject;
                GetComponent<Enemy_Patrol>().IsPatrol = false;
                return;
            }
        }
        IsChecking = false;
        IsCheckingCharacter = false;
        detected_character = null;
        GetComponent<Enemy_Patrol>().IsPatrol = true;
    }

    private void Check_Character()
    {
        if (IsCheckingCharacter)
        {
            float dis = ((Vector2)(detected_character.transform.position - transform.position)).magnitude;
            Check_Character_Bar += Check_Character_Bar_Max_Up_Speed / dis * Time.deltaTime;
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
