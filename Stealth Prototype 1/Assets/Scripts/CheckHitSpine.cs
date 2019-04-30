using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHitSpine : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HitSpine();
    }

    private void HitSpine()
    {
        GameObject Spine = GetHitSpine();
        if (Spine != null)
        {
            if (Spine.CompareTag("SpineLethal"))
            {
                if (Character_Manager.Main_Character.activeSelf && Character_Manager.Fairy.activeSelf)
                {
                    EventManager.instance.Fire(new CharacterDied(gameObject));
                }
                gameObject.SetActive(false);


            }
            else if (Spine.CompareTag("SpineKnockBack"))
            {
                EventManager.instance.Fire(new CharacterHitSpineEdge(gameObject,Spine));
            }
        }
    }

    private GameObject GetHitSpine()
    {
        GameObject Spine = null;
        var CharacterMove = GetComponent<CharacterMove>();
        if (CharacterMove.HitLeftWall)
        {
            if (CharacterMove.LeftWall.CompareTag("SpineLethal"))
            {
                Spine = CharacterMove.LeftWall;
                return Spine;
            }
            if (CharacterMove.LeftWall.CompareTag("SpineKnockBack"))
            {
                Spine = CharacterMove.LeftWall;
                return Spine;
            }
        }
        if (CharacterMove.HitRightWall)
        {
            if (CharacterMove.RightWall.CompareTag("SpineLethal"))
            {
                Spine = CharacterMove.RightWall;
                return Spine;
            }
            if (CharacterMove.RightWall.CompareTag("SpineKnockBack"))
            {
                Spine = CharacterMove.RightWall;
                return Spine;
            }
        }
        if(CharacterMove.OnGround)
        {
            if (CharacterMove.Ground.CompareTag("SpineLethal"))
            {
                Spine = CharacterMove.Ground;
                return Spine;
            }
            if (CharacterMove.Ground.CompareTag("SpineKnockBack"))
            {
                Spine = CharacterMove.Ground;
                return Spine;
            }
        }

        return Spine;
    }
}
