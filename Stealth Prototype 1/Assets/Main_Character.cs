using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character : MonoBehaviour
{

    public float ground_speed;
    public float rope_speed;

    private bool onground;
    private bool onrope;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        check_onground();
        check_input();
    }

    private void check_input()
    {
        if (onground)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += (Vector3)(ground_speed * Vector2.right * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += (Vector3)(ground_speed * Vector2.left * Time.deltaTime);
            }
        }
        if (onrope)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += (Vector3)(rope_speed * Vector2.up * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += (Vector3)(rope_speed * Vector2.down * Time.deltaTime);
            }
        }
    }

    private void check_onground()
    {
        int layermask = 1 << 9;
        layermask = ~layermask;
        RaycastHit2D hit=Physics2D.BoxCast(new Vector2(transform.position.x,transform.position.y-transform.localScale.y/2),transform.localScale,0,Vector2.down,0,layermask);
        if (hit.collider != null)
        {

            onground = true;
        }
        else
        {
            onground = false;
        }
    }

}
