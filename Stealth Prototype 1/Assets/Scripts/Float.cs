using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Float : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = ControllerManager.Fairy;
    }

    // Update is called once per frame
    void Update()
    {
        Check_Input();
    }

    private void Check_Input()
    {
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (Fairy_Status.status != FairyStatus.Aimed && Fairy_Status.status!=FairyStatus.KnockBack&& Fairy_Status.status != FairyStatus.FloatPlatform && Fairy_Status.status != FairyStatus.Climbing&&!GetComponent<CharacterMove>().OnGround&&GetComponent<CharacterMove>().speed.y<=0)
        {
            if (InputAvailable())
            {
                if(Fairy_Status.status != FairyStatus.Float)
                {
                    GetComponent<CharacterMove>().speed.y = 0;
                }
                Fairy_Status.status = FairyStatus.Float;
            }
            else
            {
                if (Fairy_Status.status == FairyStatus.Float)
                {
                    Fairy_Status.status = FairyStatus.Normal;
                }
            }
        }
        else
        {
            if (Fairy_Status.status == FairyStatus.Float)
            {
                Fairy_Status.status = FairyStatus.Normal;
            }
        }
    }

    private bool InputAvailable()
    {
        if (ControllerManager.FairyJoystick != null)
        {
            return player.GetButton("A");
        }
        else
        {
            return Input.GetKey(KeyCode.Space);
        }
    }
}
