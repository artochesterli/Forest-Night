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
        if (CharacterMove.HitWall)
        {
            Spine = CharacterMove.Wall;
        }
        else if(CharacterMove.OnGround)
        {
            Spine = CharacterMove.Ground;
        }

        return Spine;
    }
}
