using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ShootArrow : MonoBehaviour
{
    public GameObject Connected_Arrow;

    private Player player;
    private Vector2 direction;
    private List<GameObject> Aim_Line;

    private const float Velocity_Charge_Speed = 10;
    private const float Aim_offset = 1;
    private const float mirroBounceStartPointOffset = 0.05f;
    private const float AimLineUnitPerMeter = 2;
    private const float AimThreshold=0.5f;

    private const float mirrorTopDownOffset = 0.05f;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
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
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (Fairy_Status.status != Fairy_Status.FLOAT&& Fairy_Status.status != Fairy_Status.TRANSPORTING && Fairy_Status.status != Fairy_Status.FLOAT_PLATFORM && GetComponent<Check_Onground>().onground && player.GetButtonDown("LT"))
        {
            Fairy_Status.status = Fairy_Status.AIM;
            if (Connected_Arrow == null)
            {
                Connected_Arrow = (GameObject)Instantiate(Resources.Load("Prefabs/Arrow"));
            }
            direction = transform.right;
            ClearAimLine();
            CreateAimLIne(direction, Connected_Arrow.transform.position);
            Connected_Arrow.transform.position = transform.position + (Vector3)direction * Aim_offset;
        }

        if (Fairy_Status.status == Fairy_Status.AIM)
        {
            
            Vector2 temp = Vector2.zero;
            temp.x = player.GetAxis("Right Stick X");
            temp.y = player.GetAxis("Right Stick Y");
            if (temp.magnitude > AimThreshold)
            {
                direction = temp;
                direction.Normalize();
                ClearAimLine();
                CreateAimLIne(direction, Connected_Arrow.transform.position);

            }
            /*else
            {
                ClearAimLine();
                direction = Vector2.zero;
            }*/
            Connected_Arrow.transform.position = transform.position + (Vector3)direction * Aim_offset;

            if (player.GetButtonUp("LT"))
            {
                ClearAimLine();
                if (direction.magnitude > 0)
                {
                    Connected_Arrow.GetComponent<Arrow>().direction = direction;
                    Connected_Arrow.GetComponent<Arrow>().emit = true;
                    Connected_Arrow.transform.parent = null;
                    Connected_Arrow = null;
                }
                else
                {
                    Destroy(Connected_Arrow);
                }
                Fairy_Status.status = Fairy_Status.NORMAL;
            }
        }

    }

    private void CreateAimLIne(Vector2 direction, Vector2 StartPoint)
    {
        int layermask = 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Portal") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger");
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
            float MirrorTopY = hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider2D>().size.y * hit.collider.gameObject.transform.localScale.y / 2;
            if (hit.point.y < MirrorTopY-mirrorTopDownOffset)
            {
                if (direction.x > 0)
                {
                    StartPoint = hit.point + Vector2.left * mirroBounceStartPointOffset;
                }
                else
                {
                    StartPoint = hit.point + Vector2.right * mirroBounceStartPointOffset;
                }
                direction.x = -direction.x;
                CreateAimLIne(direction, StartPoint);
            }
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
