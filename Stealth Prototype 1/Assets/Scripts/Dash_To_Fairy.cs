using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Dash_To_Fairy : MonoBehaviour
{
    public bool detect_float_fairy;
    public float dash_distance;
    public float dash_speed;
    public float over_dash_velocity;
    public float over_dash_stable_time;
    public float over_dash_decelerate_time;
    public float over_dash_decelerate_factor;
    public float dash_pause_time;

    private bool collide;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
    }

    // Update is called once per frame
    void Update()
    {
        var character = GetComponent<Main_Character_Status_Manager>();
        if (character.status != character.TRANSPORTING && character.status!=character.AIMED)
        {
            lock_fairy();
            Check_Input();
        }
    }

    private void lock_fairy()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (Character_Manager.Fairy == null)
        {
            return;
        }
        if (Main_Character_Status.status==Main_Character_Status.DASHING)
        {
            detect_float_fairy = false;
            return;
        }
        GameObject fairy = Character_Manager.Fairy;
        float current_dis = ((Vector2)(transform.position) - (Vector2)(fairy.transform.position)).magnitude;
        if (current_dis <= dash_distance && fairy.GetComponent<Fairy_Status_Manager>().status==fairy.GetComponent<Fairy_Status_Manager>().FLOAT_PLATFORM)
        {
            int layermask = (1 << LayerMask.NameToLayer("Main_Character")) | (1 << LayerMask.NameToLayer("Totem"));
            layermask = ~layermask;
            Vector2 direction = fairy.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x);
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), transform.localScale*0.9f, angle, direction, dash_distance, layermask);
            if (hit.collider.gameObject.CompareTag("Fairy"))
            {
                detect_float_fairy = true;
            }
            else
            {
                detect_float_fairy = false;
            }
        }
        else
        {
            detect_float_fairy = false;
        }
        GameObject Aim_Icon = fairy.transform.Find("Aim_Icon").gameObject;
        if (detect_float_fairy)
        {
            Aim_Icon.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            Aim_Icon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Check_Input()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (player.GetButtonDown("RT") && detect_float_fairy && Main_Character_Status.status != Main_Character_Status.DASHING)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        Main_Character_Status.status = Main_Character_Status.DASHING;

        Vector2 direction = Character_Manager.Fairy.transform.position - transform.position;
        direction.Normalize();
        Vector2 target = Character_Manager.Fairy.transform.position;
        //GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float dis = (target - (Vector2)transform.position).magnitude;
        while (Vector2.Dot(direction, target - (Vector2)transform.position) > 0)
        {
            transform.position += (Vector3)(dash_speed*direction * Time.deltaTime);
            yield return null;
        }
        float over_dash_velocity_x = over_dash_velocity * (direction.x / Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y));
        float over_dash_velocity_y = over_dash_velocity * (direction.y / Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y));

        //Main_Character_Status.Status = Main_Character_Status.NORMAL;
        //GetComponent<Rigidbody2D>().velocity = new Vector2(over_dash_velocity_x, 0);
        float time_count = 0;
        //float current_speed = over_dash_velocity_y;
        float current_speed = over_dash_velocity;
        while (time_count < over_dash_stable_time)
        {
            transform.position+= (Vector3)(current_speed * direction* Time.deltaTime);
            time_count += Time.deltaTime;
            yield return null;
            if (collide)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
                Main_Character_Status.status = Main_Character_Status.NORMAL;
                yield break;
            }
        }


        //GetComponent<Rigidbody2D>().drag = 1;
        time_count = 0;

        while (time_count < over_dash_decelerate_time)
        {
            transform.position += (Vector3)(current_speed* direction * Time.deltaTime);
            current_speed -= over_dash_decelerate_factor* over_dash_velocity / over_dash_decelerate_time * Time.deltaTime;
            time_count += Time.deltaTime;
            yield return null;
            if (collide)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
                Main_Character_Status.status = Main_Character_Status.NORMAL;
                yield break;
            }
        }
        current_speed = (1 - over_dash_decelerate_factor) * over_dash_velocity;

        Main_Character_Status.status = Main_Character_Status.NORMAL;

        time_count = 0;
        while (time_count < dash_pause_time)
        {
            transform.position += (Vector3)(current_speed * direction * Time.deltaTime);
            time_count += Time.deltaTime;
            yield return null;
        }
        
        //GetComponent<Rigidbody2D>().drag = 0;
        //GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collide = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collide = false;
    }
}
