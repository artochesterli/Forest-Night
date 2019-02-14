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

    private bool dashing;

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
    }

    private void lock_fairy()
    {
        if (dashing)
        {
            detect_float_fairy = false;
            return;
        }
        GameObject fairy = Character_Manager.Fairy;
        float current_dis = ((Vector2)(transform.position) - (Vector2)(fairy.transform.position)).magnitude;
        if (current_dis <= dash_distance && fairy.GetComponent<Float_Point>().Is_Float_Point)
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
        if (player.GetButtonDown("Dash and Release") && detect_float_fairy && !dashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        dashing = true;
        Vector2 direction = Character_Manager.Fairy.transform.position - transform.position;
        direction.Normalize();
        Vector2 target = Character_Manager.Fairy.transform.position;
        while (Vector2.Dot(direction, target - (Vector2)transform.position) > 0)
        {
            transform.position += (Vector3)(dash_speed*direction * Time.deltaTime);
            yield return null;
        }

    }
}
