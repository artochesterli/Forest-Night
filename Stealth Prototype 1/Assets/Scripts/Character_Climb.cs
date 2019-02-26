using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Climb : MonoBehaviour
{

    public float speed;
    public float jump_off_velocity;

    private Player player;
    private bool IsClimbing;
    private bool in_Path_end;
    private bool AbleToClimb;
    private GameObject connected_path;
    private const float climb_initial_offset = 0.2f;
    private const float climb_threshold = 0.5f;
    private const float climb_jump_threshold = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
        connected_path = null;
        in_Path_end = false;
        IsClimbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Check_Input();
        Check_Status();
    }

    private void Check_Input()
    {
        if ( AbleToClimb && !IsClimbing && (player.GetAxis("Left Stick Y") > climb_threshold && connected_path.transform.position.y>transform.position.y|| player.GetAxis("Left Stick Y") < -climb_threshold && connected_path.transform.position.y < transform.position.y))
        {
            IsClimbing = true;
            if (gameObject.CompareTag("Fairy"))
            {
                var Status = GetComponent<Fairy_Status_Manager>();
                if (player.GetAxis("Left Stick Y") > 0)
                {
                    transform.position = new Vector3(connected_path.transform.position.x, transform.position.y + climb_initial_offset, 0);
                }
                else
                {
                    transform.position = new Vector3(connected_path.transform.position.x, transform.position.y - climb_initial_offset, 0);
                }
                Status.status = Status.CLIMBING;

            }
            else if (gameObject.CompareTag("Main_Character"))
            {
                var Status = GetComponent<Main_Character_Status_Manager>();
                if (player.GetAxis("Left Stick Y") > 0)
                {
                    transform.position = new Vector3(connected_path.transform.position.x, transform.position.y + climb_initial_offset, 0);
                }
                else
                {
                    transform.position = new Vector3(connected_path.transform.position.x, transform.position.y - climb_initial_offset, 0);
                }
                Status.Status = Status.CLIMBING;
            }
        }
        if (!AbleToClimb)
        {
            IsClimbing = false;
        }

        if(IsClimbing&&Mathf.Abs(player.GetAxis("Left Stick Y")) > climb_threshold)
        {
            Vector2 moveVector = Vector2.zero;
            moveVector.y = player.GetAxis("Left Stick Y");
            moveVector.Normalize();
            transform.position += (Vector3)moveVector * speed * Time.deltaTime;
        }

        if (IsClimbing)
        {
            if(player.GetAxis("Left Stick X") > climb_jump_threshold)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if(player.GetAxis("Left Stick X") < -climb_jump_threshold)
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            if (player.GetButtonDown("A") && !in_Path_end)
            {
                IsClimbing = false;
                GetComponent<Rigidbody2D>().velocity += (Vector2)transform.right * jump_off_velocity;
            }
        }
    }

    private void Check_Status()
    {
        if (IsClimbing)
        {

            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            if (gameObject.CompareTag("Fairy"))
            {
                var Status = GetComponent<Fairy_Status_Manager>();
                if (Status.status == Status.CLIMBING)
                {
                    Status.status = Status.NORMAL;
                }

            }
            else if (gameObject.CompareTag("Main_Character"))
            {
                var Status = GetComponent<Main_Character_Status_Manager>();
                if (Status.Status == Status.CLIMBING)
                {
                    Status.Status = Status.NORMAL;
                }
            }
        }
        if (in_Path_end)
        {

        }
        else
        {
            
            if (GetComponent<Check_Onground>().onground)
            {
                IsClimbing = false;
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Path"))
        {
            connected_path = ob;
            AbleToClimb = true;
        }
        if (ob.name == "Path_End")
        {
            in_Path_end = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Path"))
        {
            connected_path = null;
            AbleToClimb = false;
        }
        if (ob.name == "Path_End")
        {
            in_Path_end = false;
        }
    }
}
