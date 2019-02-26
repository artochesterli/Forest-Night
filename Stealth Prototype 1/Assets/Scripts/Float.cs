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
        if (Fairy_Status.status != Fairy_Status.FLOAT_PLATFORM && Fairy_Status.status != Fairy_Status.CLIMBING&&!GetComponent<Check_Onground>().onground&&GetComponent<Rigidbody2D>().velocity.y<=0)
        {
            if (player.GetButton("A"))
            {
                if(Fairy_Status.status != Fairy_Status.FLOAT)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                }
                Fairy_Status.status = Fairy_Status.FLOAT;
            }
            else
            {
                if (Fairy_Status.status == Fairy_Status.FLOAT)
                {
                    Fairy_Status.status = Fairy_Status.NORMAL;
                }
            }
        }
        else
        {
            if (Fairy_Status.status == Fairy_Status.FLOAT)
            {
                Fairy_Status.status = Fairy_Status.NORMAL;
            }
        }
    }
}
