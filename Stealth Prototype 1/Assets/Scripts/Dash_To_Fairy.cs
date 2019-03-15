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
    public float OverDashForce;
    public float OverDashUnlockThreshold;

    private bool OverDash;
    private float OverDashProportion;

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
        CheckOverDashFree();
    }

    private void FixedUpdate()
    {
        CheckOverDashFree();
    }

    private void lock_fairy()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (Character_Manager.Fairy == null)
        {
            return;
        }
        GameObject Aim_Icon = Character_Manager.Fairy.transform.Find("Aim_Icon").gameObject;
        if (detect_float_fairy)
        {
            Aim_Icon.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            Aim_Icon.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Main_Character_Status.status==Main_Character_Status.DASHING || OverDash)
        {
            detect_float_fairy = false;
            return;
        }
        GameObject fairy = Character_Manager.Fairy;
        float current_dis = ((Vector2)(transform.position) - (Vector2)(fairy.transform.position)).magnitude;
        if (current_dis <= dash_distance && fairy.GetComponent<Fairy_Status_Manager>().status==fairy.GetComponent<Fairy_Status_Manager>().FLOAT_PLATFORM)
        {
            int layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Totem") | 1<<LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger");
            layermask = ~layermask;
            Vector2 direction = fairy.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction, dash_distance, layermask);
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
        GetComponent<Rigidbody2D>().gravityScale = 0;


        Vector2 direction = Character_Manager.Fairy.transform.position - transform.position;
        direction.Normalize();
        Vector2 target = Character_Manager.Fairy.transform.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float dis = (target - (Vector2)transform.position).magnitude;
        while (Vector2.Dot(direction, target - (Vector2)transform.position) > 0)
        {
            transform.position += (Vector3)(dash_speed*direction * Time.deltaTime);
            yield return null;
        }

        float current_speed = over_dash_velocity;

        OverDash = true;
        GetComponent<Rigidbody2D>().velocity = current_speed * direction;

    }

    private void CheckOverDashFree()
    {
        if (OverDash)
        {
            var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
            var rb = GetComponent<Rigidbody2D>();
            if (collide)
            {
                rb.velocity = Vector2.zero;
                Main_Character_Status.status = Main_Character_Status.NORMAL;
                OverDash = false;
            }


            if (rb.velocity.y > 0)
            {
                rb.AddForce(-OverDashForce * rb.velocity.normalized);
            }
            else
            {
                if (rb.velocity.x > 0)
                {
                    rb.AddForce(OverDashForce * OverDashProportion * Vector2.left);
                }
                else
                {
                    rb.AddForce(OverDashForce * OverDashProportion * Vector2.right);
                }
                
            }
            
            if (rb.velocity.magnitude < OverDashUnlockThreshold)
            {
                if (Main_Character_Status.status == Main_Character_Status.DASHING)
                {
                    OverDashProportion = Mathf.Abs(rb.velocity.x) / Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);
                    Main_Character_Status.status = Main_Character_Status.NORMAL;
                }
            }
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
                OverDash = false;
            }

        }
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
