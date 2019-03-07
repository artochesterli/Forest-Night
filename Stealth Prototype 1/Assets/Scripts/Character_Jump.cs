using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Jump : MonoBehaviour
{

    public float jump_initial_velocity_y;

    private bool AbleToJump;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
        
    }

    // Update is called once per frame
    void Update()
    {
        check_avaliability();
        check_jump();
    }

    private void check_jump()
    {
        var check_onground = GetComponent<Check_Onground>();
        if (player.GetButtonDown("A") && check_onground.onground&&AbleToJump)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(0, jump_initial_velocity_y);
        }

    }

    private void check_avaliability()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            var Fairy_Status = GetComponent<Fairy_Status_Manager>();
            if (Fairy_Status.status == Fairy_Status.NORMAL)
            {
                AbleToJump = true;
            }
            else
            {
                AbleToJump = false;
            }
        }
        else if (gameObject.CompareTag("Main_Character"))
        {
            var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
            if (Main_Character_Status.status == Main_Character_Status.NORMAL)
            {
                AbleToJump = true;
            }
            else
            {
                AbleToJump = false;
            }

        }
    }

}
