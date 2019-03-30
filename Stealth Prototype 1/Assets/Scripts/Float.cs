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
        player = GetComponent<PlayerId>().player;
    }

    // Update is called once per frame
    void Update()
    {
        Check_Input();
    }

    private void Check_Input()
    {
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (Fairy_Status.status != FairyStatus.Aimed && Fairy_Status.status != FairyStatus.FloatPlatform && Fairy_Status.status != FairyStatus.Climbing&&!GetComponent<CharacterMove>().OnGround&&GetComponent<CharacterMove>().speed.y<=0)
        {
            if (player.GetButton("A"))
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
}
