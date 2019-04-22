using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyAnimationManager : MonoBehaviour
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
        var state = GetComponent<Fairy_Status_Manager>();
        var CharacterMove = GetComponent<CharacterMove>();

        

        if (state.status == FairyStatus.Aimed)
        {
            if (CharacterMove.OnGround)
            {
                if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyGroundAimed"))
                {
                    GetComponent<Animator>().Play("FairyGroundAimed" ,0 ,0);
                }
            }
            else
            {
                if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyAirAimed"))
                {
                    GetComponent<Animator>().Play("FairyAirAimed" ,0 ,0);
                }
            }
            return;
        }

        if (state.status == FairyStatus.Normal && CharacterMove.OnGround && Mathf.Abs(CharacterMove.speed.x) > 0)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyWalk"))
            {
                GetComponent<Animator>().Play("FairyWalk" ,0 ,0);
            }
            return;
        }
        if (state.status == FairyStatus.Normal && CharacterMove.speed == Vector2.zero && CharacterMove.OnGround)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyIdle"))
            {
                GetComponent<Animator>().Play("FairyIdle" ,0 ,0);
            }
            return;
        }
        
        if(state.status == FairyStatus.Normal && !CharacterMove.OnGround && CharacterMove.speed.y > 0)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyJumpUp"))
            {
                GetComponent<Animator>().Play("FairyJumpUp" ,0 ,0);
            }
            return;
        }

        if (state.status == FairyStatus.Normal && !CharacterMove.OnGround && CharacterMove.speed.y < 0)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyJumpDown"))
            {
                GetComponent<Animator>().Play("FairyJumpDown" ,0 ,0);
            }
            return;
        }

        if(state.status == FairyStatus.Float)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyGlide"))
            {
                GetComponent<Animator>().Play("FairyGlide" ,0 ,0);
            }
            return;
        }

        if(state.status == FairyStatus.FloatPlatform)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FairyFloatPoint"))
            {
                GetComponent<Animator>().Play("FairyFloatPoint" ,0 ,0);
            }
            return;
        }

    }
}
