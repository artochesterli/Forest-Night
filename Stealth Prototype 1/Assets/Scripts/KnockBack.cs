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
            if (Grounded || GetComponent<CharacterMove>().HitLeftWall || GetComponent<CharacterMove>().HitRightWall || GetComponent<CharacterMove>().HitTop || GetComponent<CharacterMove>().OnGround)
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

}
