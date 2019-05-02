using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        var state = GetComponent<Main_Character_Status_Manager>();
        var CharacterMove = GetComponent<CharacterMove>();

        GetComponent<Animator>().speed = 1;
        if (GetComponent<Slash>().Slashing)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterSlash"))
            {
                GetComponent<Animator>().Play("MainCharacterSlash" ,0 ,0);
            }
            return;
        }

        if (state.status == MainCharacterStatus.Normal && CharacterMove.OnGround && Mathf.Abs(CharacterMove.speed.x) > 0)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterWalk"))
            {
                GetComponent<Animator>().Play("MainCharacterWalk" ,0 ,0);
            }
            return;
        }
        if (state.status == MainCharacterStatus.Normal && CharacterMove.speed == Vector2.zero && CharacterMove.OnGround)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterIdle"))
            {
                GetComponent<Animator>().Play("MainCharacterIdle" ,0 ,0);
            }
            return;
        }

        if (state.status == MainCharacterStatus.Normal && !CharacterMove.OnGround && CharacterMove.speed.y > 0)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterJumpUp"))
            {
                GetComponent<Animator>().Play("MainCharacterJumpUp" ,0 ,0);
            }
            return;
        }

        if (state.status == MainCharacterStatus.Normal && !CharacterMove.OnGround && CharacterMove.speed.y < 0)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterJumpDown"))
            {
                GetComponent<Animator>().Play("MainCharacterJumpDown" ,0 ,0);
            }
            return;
        }

        if(state.status == MainCharacterStatus.Dashing || state.status == MainCharacterStatus.OverDash)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterDash"))
            {
                GetComponent<Animator>().Play("MainCharacterDash");
            }
            return;
        }

        if (state.status == MainCharacterStatus.Climbing)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterClimb"))
            {
                GetComponent<Animator>().Play("MainCharacterClimb", 0, 0);
            }
            else
            {
                if (CharacterMove.speed.y != 0)
                {
                    GetComponent<Animator>().speed = 1;
                }
                else
                {
                    GetComponent<Animator>().speed = 0;
                }
            }
        }

        if (state.status == MainCharacterStatus.Aimed)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MainCharacterAimed"))
            {
                GetComponent<Animator>().Play("MainCharacterAimed", 0, 0);
            }
        }


    }
}
