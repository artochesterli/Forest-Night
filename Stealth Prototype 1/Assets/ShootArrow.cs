using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ShootArrow : MonoBehaviour
{
    public GameObject Connected_Arrow;

    private float current_arrow_velocity;
    private Player player;

    private const float Velocity_Charge_Speed = 10;
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
        direction.x = player.GetAxis("Select Direction X");
        direction.y = player.GetAxis("Select Direction Y");
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (player.GetButton("Charge")&&direction != Vector2.zero&&Fairy_Status.status!=Fairy_Status.FLOAT)
        {
            direction.Normalize();
            Fairy_Status.status = Fairy_Status.AIM;
            if (Connected_Arrow == null)
            {
                Connected_Arrow = (GameObject)Instantiate(Resources.Load("Prefabs/Arrow"));
            }
            Connected_Arrow.transform.position = transform.position + (Vector3)direction;
            current_arrow_velocity += Velocity_Charge_Speed * Time.deltaTime;
            if (current_arrow_velocity > Max_Arrow_Velocity)
            {
                current_arrow_velocity = Max_Arrow_Velocity;
            }

        }
        else
        {
            if (Connected_Arrow != null)
            {
                Destroy(Connected_Arrow.gameObject);
            }
        }


    }

}
