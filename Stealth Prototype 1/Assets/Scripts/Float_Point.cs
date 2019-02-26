using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Float_Point : MonoBehaviour
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
        if (player.GetButton("LT")&& Fairy_Status.status!=Fairy_Status.CLIMBING&&!GetComponent<Check_Onground>().onground)
        {
            Fairy_Status.status = Fairy_Status.FLOAT_PLATFORM;
        }
        else
        {
            if (Fairy_Status.status == Fairy_Status.FLOAT_PLATFORM)
            {
                Fairy_Status.status = Fairy_Status.NORMAL;
            }
        }
    }


}
