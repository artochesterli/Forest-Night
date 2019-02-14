using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Horizontal_Movement : MonoBehaviour
{

    public float ground_speed;
    public float air_speed;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;

    }

    // Update is called once per frame
    void Update()
    {
        check_input();
    }

    private void check_input()
    {
        var check_onground = GetComponent<Check_Onground>();
        Vector3 moveVector=Vector3.zero;
        moveVector.x = player.GetAxis("Move Horizontal");
        if (check_onground.onground)
        {
            transform.position += moveVector * ground_speed * Time.deltaTime;
        }
        else
        {
            transform.position += moveVector * air_speed * Time.deltaTime;
        }
        if (player.id== 1)
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (check_onground.onground)
                {
                    transform.position += ground_speed * Vector3.right * Time.deltaTime;
                }
                else
                {
                    transform.position += air_speed * Vector3.right * Time.deltaTime;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (check_onground.onground)
                {
                    transform.position += ground_speed * Vector3.left * Time.deltaTime;
                }
                else
                {
                    transform.position += air_speed * Vector3.right * Time.deltaTime;
                }
            }
        }
    }
}
