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
    public float dash_pause_time;


    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
    }

    // Update is called once per frame
    void Update()
    {
        lock_fairy();
        Check_Input();
    }

    private void lock_fairy()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (Character_Manager.Fairy == null)
        {
            return;
        }
        if (Main_Character_Status.Status==Main_Character_Status.DASHING)
        {
            detect_float_fairy = false;
            return;
        }
        GameObject fairy = Character_Manager.Fairy;
        float current_dis = ((Vector2)(transform.position) - (Vector2)(fairy.transform.position)).magnitude;
        if (current_dis <= dash_distance && fairy.GetComponent<Fairy_Status_Manager>().status==fairy.GetComponent<Fairy_Status_Manager>().FLOAT)
        {
            int layermask = (1 << LayerMask.NameToLayer("Main_Character")) | (1 << LayerMask.NameToLayer("Invisible_Ward"));
            layermask = ~layermask;
            Vector2 direction = fairy.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x);
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), transform.localScale, angle, direction, dash_distance, layermask);
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
        if (detect_float_fairy)
        {
            fairy.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            fairy.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void Check_Input()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (player.GetButtonDown("Dash and Release") && detect_float_fairy && Main_Character_Status.Status != Main_Character_Status.DASHING)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        Main_Character_Status.Status = Main_Character_Status.DASHING;

        Vector2 direction = Character_Manager.Fairy.transform.position - transform.position;
        direction.Normalize();
        Vector2 target = Character_Manager.Fairy.transform.position;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float dis = (target - (Vector2)transform.position).magnitude;
        while (Vector2.Dot(direction, target - (Vector2)transform.position) > 0)
        {
            transform.position += (Vector3)(dash_speed*direction * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(dash_pause_time);

        Main_Character_Status.Status = Main_Character_Status.NORMAL;
        GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
        //Character_Manager.Fairy.GetComponent<Float_Point>().Is_Float_Point = false;
        GetComponent<Rigidbody2D>().velocity = direction * over_dash_velocity;
        

    }
}
