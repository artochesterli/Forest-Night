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
    private const float Aim_offset = 1;
    private const float mirroBounceStartPointOffset = 0.01f;
    private const float AimLineUnitPerMeter = 2;
    private List<GameObject> Aim_Line;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
        current_arrow_velocity = 0;
        Aim_Line = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        var status = GetComponent<Fairy_Status_Manager>();
        if (status.status != status.TRANSPORTING&&status.status!=status.AIMED)
        {
            Check_Input();
        }
    }

    private void Check_Input()
    {
        Vector2 direction = Vector2.zero;
        direction.x = player.GetAxis("Right Stick X");
        direction.y = player.GetAxis("Right Stick Y");
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (direction != Vector2.zero&&Fairy_Status.status!=Fairy_Status.FLOAT&&Fairy_Status.status!=Fairy_Status.TRANSPORTING&&Fairy_Status.status!=Fairy_Status.FLOAT_PLATFORM&&GetComponent<Check_Onground>().onground)
        {
            direction.Normalize();
            Fairy_Status.status = Fairy_Status.AIM;
            if (Connected_Arrow == null)
            {
                Connected_Arrow = (GameObject)Instantiate(Resources.Load("Prefabs/Arrow"));
            }
            Connected_Arrow.transform.position = transform.position + (Vector3)direction*Aim_offset;
            Connected_Arrow.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, direction),Vector3.forward);

            ClearAimLine();
            CreateAimLIne(direction, Connected_Arrow.transform.position);

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
            ClearAimLine();
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

    private void CreateAimLIne(Vector2 direction, Vector2 StartPoint)
    {
        int layermask = 1 << LayerMask.NameToLayer("Bullet") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Portal");
        layermask = ~layermask;
        float mag = 100;
        RaycastHit2D hit= Physics2D.Raycast(StartPoint, direction, mag, layermask);
        int num = Mathf.FloorToInt((hit.point - StartPoint).magnitude*AimLineUnitPerMeter)+1;
        for(int i = 0; i < num; i++)
        {
            GameObject unit = (GameObject)Instantiate(Resources.Load("Prefabs/AimLineUnit"), StartPoint + direction * (1.0f/AimLineUnitPerMeter)*i, new Quaternion(0, 0, 0, 0));
            Aim_Line.Add(unit);
        }
        if (hit.collider.gameObject.CompareTag("Mirror"))
        {
            StartPoint = hit.point - Vector2.one * direction.x * mirroBounceStartPointOffset;
            direction.x = -direction.x;
            CreateAimLIne(direction, StartPoint);
        }

    }

    private void ClearAimLine()
    {
        for (int i = 0; i < Aim_Line.Count; i++)
        {
            Destroy(Aim_Line[i]);
        }
    }

}
