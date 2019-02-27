﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ShootArrow : MonoBehaviour
{
    public GameObject Connected_Arrow;

    private float current_arrow_velocity;
    private Player player;

    private const float Velocity_Charge_Speed = 10;
    private const float Min_Arrow_Velocity = 5;
    private const float Max_Arrow_Velocity = 20;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
        current_arrow_velocity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Check_Input();
    }

    private void Check_Input()
    {
        Vector2 direction = Vector2.zero;
        direction.x = player.GetAxis("Right Stick X");
        direction.y = player.GetAxis("Right Stick Y");
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (direction != Vector2.zero&&Fairy_Status.status!=Fairy_Status.FLOAT)
        {
            direction.Normalize();
            Fairy_Status.status = Fairy_Status.AIM;
            GameObject Aim_Line = transform.Find("Aim_Line").gameObject;
            Aim_Line.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            Aim_Line.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction), Vector3.forward);
            if (Connected_Arrow == null)
            {
                Connected_Arrow = (GameObject)Instantiate(Resources.Load("Prefabs/Arrow"));
            }
            Connected_Arrow.transform.position = transform.position + (Vector3)direction;

            Connected_Arrow.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction),Vector3.forward);
            if (player.GetButtonDown("RT"))
            {
                Connected_Arrow.GetComponent<Arrow>().direction = direction;

                current_arrow_velocity = 0;
                Connected_Arrow.transform.parent = null;
                Connected_Arrow = null;
            }
        }
        else
        {
            GameObject Aim_Line = transform.Find("Aim_Line").gameObject;
            Aim_Line.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            if (Fairy_Status.status == Fairy_Status.AIM)
            {
                Fairy_Status.status = Fairy_Status.NORMAL;
            }
            if (Connected_Arrow != null)
            {
                Destroy(Connected_Arrow.gameObject);
            }
        }
    }

    

}
