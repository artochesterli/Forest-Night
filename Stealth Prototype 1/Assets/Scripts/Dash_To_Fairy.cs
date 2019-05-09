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

    private GameObject SpiritLine;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = ControllerManager.MainCharacter;
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDisable()
    {
        Destroy(SpiritLine);
        detect_float_fairy = false;
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
        else
        {
            Destroy(SpiritLine);
            detect_float_fairy = false;
        }
        CheckOverDash();
    }

    private void CreateSpiritLine()
    {
        SpiritLine = (GameObject)Instantiate(Resources.Load("Prefabs/VFX/SpiritLine"));
        Vector3 Start = transform.position;
        Vector3 End = Character_Manager.Fairy.transform.position + (Vector3)Character_Manager.Fairy.GetComponent<Float_Point>().DashOffset;
        foreach(Transform child in SpiritLine.transform)
        {
            child.GetComponent<LineRenderer>().SetPosition(0, -(End - Start).magnitude / 2 * Vector3.right);
            child.GetComponent<LineRenderer>().SetPosition(1, (End - Start).magnitude / 2 * Vector3.right);
        }
        SpiritLine.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, Start-End));
        SpiritLine.transform.position = (End + Start) / 2;
    }

    private void lock_fairy()
    {
        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        if (detect_float_fairy)
        {
            Destroy(SpiritLine);
            CreateSpiritLine();
        }
        else
        {
            Destroy(SpiritLine);
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
            if (hit && hit.collider.gameObject.CompareTag("Fairy"))
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
        transform.Find("DashEffect").GetComponent<AudioSource>().Play();

        detect_float_fairy = false;
        Destroy(SpiritLine);

        var Main_Character_Status = GetComponent<Main_Character_Status_Manager>();
        
        Main_Character_Status.status = MainCharacterStatus.Dashing;

        Vector2 direction = (Vector2)(Character_Manager.Fairy.transform.position - transform.position) + Character_Manager.Fairy.GetComponent<Float_Point>().DashOffset;
        direction.Normalize();
        Vector2 target = (Vector2)Character_Manager.Fairy.transform.position + Character_Manager.Fairy.GetComponent<Float_Point>().DashOffset;
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        while (true)
        {
            GetComponent<CharacterMove>().speed = Vector2.zero;
            GetComponent<CharacterMove>().DashSpeed = dash_speed * direction;
            if (Main_Character_Status.status != MainCharacterStatus.Dashing)
            {
                yield break;
            }
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
            if (CharacterMove.DashSpeed == Vector2.zero)
            {
                Main_Character_Status.status = MainCharacterStatus.Normal;
                return;
            }
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

    private void OnLoadLevel(LoadLevel L)
    {
        Destroy(SpiritLine);
        detect_float_fairy = false;
    }

}
