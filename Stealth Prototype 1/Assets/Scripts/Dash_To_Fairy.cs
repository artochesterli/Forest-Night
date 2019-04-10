using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Dash_To_Fairy : MonoBehaviour
{
    public bool detect_float_fairy;
    public float dash_distance;
    public float dash_speed;
    public float over_dash_velocity;
    public float OverDashDeceleration;


    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
    }

    // Update is called once per frame
    void Update()
    {
        var character = GetComponent<Main_Character_Status_Manager>();
        if (character.status==MainCharacterStatus.Normal || (character.status==MainCharacterStatus.Climbing && !GetComponent<Character_Climb>().PathEndThrough))
        {
            lock_fairy();
            Check_Input();
        }
        CheckOverDash();
    }



    private void lock_fairy()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (Character_Manager.Fairy == null)
        {
            return;
        }
        GameObject Aim_Icon = Character_Manager.Fairy.transform.Find("Aim_Icon").gameObject;
        if (detect_float_fairy)
        {
            Aim_Icon.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            Aim_Icon.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Main_Character_Status.status==MainCharacterStatus.Dashing || Main_Character_Status.status == MainCharacterStatus.OverDash)
        {
            detect_float_fairy = false;
            return;
        }
        GameObject fairy = Character_Manager.Fairy;
        float current_dis = ((Vector2)(transform.position) - (Vector2)(fairy.transform.position)).magnitude;
        if (current_dis <= dash_distance && fairy.GetComponent<Fairy_Status_Manager>().status == FairyStatus.FloatPlatform)
        {
            int layermask = 1 << LayerMask.NameToLayer("Main_Character")  | 1<<LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger") | 1<<LayerMask.NameToLayer("Path");
            layermask = ~layermask;
            Vector2 direction = fairy.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction, dash_distance, layermask);
            if (hit.collider.gameObject.CompareTag("Fairy"))
            {
                detect_float_fairy = true;
            }
            else
            {
                detect_float_fairy = false;
            }
        }
        else
        {
            detect_float_fairy = false;
        }
        
    }

    private void Check_Input()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (player.GetButtonDown("RT") && detect_float_fairy && Main_Character_Status.status != MainCharacterStatus.Dashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        
        Main_Character_Status.status = MainCharacterStatus.Dashing;

        Vector2 direction = Character_Manager.Fairy.transform.position - transform.position;
        direction.Normalize();
        Vector2 target = Character_Manager.Fairy.transform.position;
        float dis = (target - (Vector2)transform.position).magnitude;
        while (true)
        {
            GetComponent<CharacterMove>().speed = Vector2.zero;
            GetComponent<CharacterMove>().DashSpeed = dash_speed * direction;
            if(Vector2.Dot(direction, target - (Vector2)transform.position) < 0)
            {
                transform.position = target;
                break;
            }
            yield return null;
        }
        Main_Character_Status.status = MainCharacterStatus.OverDash;

    }

    private void CheckOverDash()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        var CharacterMove = GetComponent<CharacterMove>();
        if (Main_Character_Status.status == MainCharacterStatus.OverDash)
        {

            Vector2 OriDashSpeed = new Vector2(CharacterMove.DashSpeed.x, CharacterMove.DashSpeed.y);
            CharacterMove.DashSpeed -= CharacterMove.DashSpeed.normalized * OverDashDeceleration * Time.deltaTime;
            if (Vector2.Dot(OriDashSpeed, CharacterMove.DashSpeed) < 0)
            {
                CharacterMove.DashSpeed = Vector2.zero;
                Main_Character_Status.status = MainCharacterStatus.Normal;
                return;
            }
        }
        else if(Main_Character_Status.status != MainCharacterStatus.Dashing)
        {
            CharacterMove.DashSpeed = Vector2.zero;
        }
    }

}
