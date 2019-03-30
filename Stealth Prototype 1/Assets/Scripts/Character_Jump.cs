using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Jump : MonoBehaviour
{

    public float JumpVerticalSpeed;

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
        if (player.GetButtonDown("A") && GetComponent<CharacterMove>().OnGround && AbleToJump)
        {
            GetComponent<CharacterMove>().speed.y = JumpVerticalSpeed;
        }

    }

    private void check_avaliability()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            var Fairy_Status = GetComponent<Fairy_Status_Manager>();
            if (Fairy_Status.status == FairyStatus.Normal)
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
            if (Main_Character_Status.status == MainCharacterStatus.Normal)
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
