using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Horizontal_Movement : MonoBehaviour
{

    public float ground_speed;
    public float air_speed;

    private Player player;

    private const float moveVectorThreshold = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            var Fairy_Status = GetComponent<Fairy_Status_Manager>();
            if (Fairy_Status.status != Fairy_Status.CLIMBING&&Fairy_Status.status!=Fairy_Status.FLOAT_PLATFORM&&Fairy_Status.status!=Fairy_Status.TRANSPORTING&&Fairy_Status.status!=Fairy_Status.AIMED)
            {
                check_input();
            }
        }
        else if(gameObject.CompareTag("Main_Character"))
        {
            var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
            if (Main_Character_Status.status == Main_Character_Status.NORMAL)
            {
                check_input();
            }
            
        }
    }

    private void check_input()
    {
        var check_onground = GetComponent<Check_Onground>();
        var check_horizontal_collider = GetComponent<CheckHorizontalCollider>();
        Vector3 moveVector=Vector3.zero;
        moveVector.x = player.GetAxis("Left Stick X");
        if (Mathf.Abs(moveVector.x) < moveVectorThreshold)
        {
            moveVector.x = 0;
        }
        else
        {
            moveVector.Normalize();
        }
        if (check_onground.onground)
        {
            if (moveVector.x > 0 && !check_horizontal_collider.RightCollide || moveVector.x < 0 && !check_horizontal_collider.LeftCollide)
            {
                transform.position += moveVector * ground_speed * Time.deltaTime;
            }
        }
        else
        {
            if (moveVector.x > 0 && !check_horizontal_collider.RightCollide || moveVector.x < 0 && !check_horizontal_collider.LeftCollide)
            {
                transform.position += moveVector * air_speed * Time.deltaTime;
            }
        }
        if (moveVector.x > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
        else if(moveVector.x < 0&&!check_horizontal_collider.LeftCollide)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }

    }
}
