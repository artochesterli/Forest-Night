using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public Vector2 KnockBackSpeed;
    public float FreeHeight;
    public Vector2 KnockBackDirection;

    
    private bool LeaveGround;
    private bool Grounded;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<CharacterHitSpineEdge>(OnHitSpineEdge);
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<CharacterHitSpineEdge>(OnHitSpineEdge);
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsKnockingBack())
        {
            if (KnockBackDirection.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
            if (transform.position.y>FreeHeight)
            {
                LeaveGround = true;
            }
            else
            {
                if (LeaveGround)
                {
                    Grounded = true;
                }
            }
            if (StopKnockBack())
            {
                GetComponent<CharacterMove>().speed.x = 0;
                LeaveGround = false;
                Grounded = false;
                if (CompareTag("Fairy"))
                {
                    GetComponent<Fairy_Status_Manager>().status = FairyStatus.Normal;
                }
                else if (CompareTag("Main_Character"))
                {
                    GetComponent<Main_Character_Status_Manager>().status = MainCharacterStatus.Normal;
                }
            }
        }
    }

    private bool StopKnockBack()
    {
        if(GetComponent<CharacterMove>().HitLeftWall && KnockBackDirection.x<0 || GetComponent<CharacterMove>().HitRightWall && KnockBackDirection.x > 0)
        {
            return true;
        }
        if (GetComponent<CharacterMove>().HitTop)
        {
            return true;
        }
        if (Grounded || GetComponent<CharacterMove>().OnGround && !GetComponent<CharacterMove>().Ground.CompareTag("SpineKnockBack"))
        {
            return true;
        }
        return false;

    }

    private bool IsKnockingBack()
    {
        if (CompareTag("Fairy"))
        {
            if (GetComponent<Fairy_Status_Manager>().status == FairyStatus.KnockBack)
            {
                return true;
            }
        }
        else if(CompareTag("Main_Character"))
        {
            if (GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.KnockBack)
            {
                return true;
            }
        }

        return false;
    }

    private void OnLoadLevel(LoadLevel L)
    {
        FreeHeight = 0;
        LeaveGround = false;
        Grounded = false;
    }

    private void OnHitSpineEdge(CharacterHitSpineEdge C)
    {
        if (C.Character == gameObject)
        {
            KnockBackDirection = C.Spine.GetComponent<KnockBackSpine>().KnockBackDirection;
            FreeHeight = C.Spine.GetComponent<KnockBackSpine>().FreeHeightOffset + transform.position.y;
        }
    }

}
