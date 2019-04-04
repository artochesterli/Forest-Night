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
                EventManager.instance.Fire(new CharacterDied(gameObject));
                Destroy(gameObject);
            }
            else if (Spine.CompareTag("SpineKnockBack"))
            {
                EventManager.instance.Fire(new CharacterHitSpineEdge(gameObject));
                GetComponent<KnockBack>().KnockBackDirection = Spine.GetComponent<KnockBackSpine>().KnockBackDirection;
                GetComponent<KnockBack>().FreeHeight = Spine.GetComponent<KnockBackSpine>().FreeHeightOffset + transform.position.y;
                if (CompareTag("Fairy"))
                {
                    GetComponent<Fairy_Status_Manager>().status = FairyStatus.KnockBack;
                }
                else if (CompareTag("Main_Character"))
                {
                    GetComponent<Main_Character_Status_Manager>().status = MainCharacterStatus.KnockBack;
                }
                GetComponent<CharacterMove>().speed.x = GetComponent<KnockBack>().KnockBackSpeed.x * Spine.GetComponent<KnockBackSpine>().KnockBackDirection.x;
                GetComponent<CharacterMove>().speed.y = GetComponent<KnockBack>().KnockBackSpeed.y * Spine.GetComponent<KnockBackSpine>().KnockBackDirection.y;
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
