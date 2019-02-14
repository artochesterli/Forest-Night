using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Jump : MonoBehaviour
{

    public float jump_initial_velocity_y;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.id == 0 && !GetComponent<Dash_To_Fairy>().dashing)
        {
            check_jump();
        }
    }

    private void check_jump()
    {
        var check_onground = GetComponent<Check_Onground>();
        if (player.GetButtonDown("Jump") && check_onground.onground)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(0, jump_initial_velocity_y);
        }

        if (player.id == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) && check_onground.onground)
            {
                GetComponent<Rigidbody2D>().velocity += new Vector2(0, jump_initial_velocity_y);
            }
        }
    }


}
