using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Slash : MonoBehaviour
{
    public bool Slashing;

    private Player player;
    private GameObject Weapon;
    private float Weapon_Active_Time_count;

    private const float Weapon_Damage_Time = 0.1f;
    private const float Weapon_Active_Time=0.2f;
    

    // Start is called before the first frame update
    void Start()
    {
        player = ControllerManager.MainCharacter;
        Weapon = transform.Find("Slash").gameObject;
        Slashing = false;
        Weapon.GetComponent<BoxCollider2D>().enabled = false;
        Weapon.GetComponent<SpriteRenderer>().enabled = false;
        Weapon_Active_Time_count = 0;
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        var character = GetComponent<Main_Character_Status_Manager>();
        if (character.status == MainCharacterStatus.Normal || character.status==MainCharacterStatus.OverDash)
        {
            Check_Input();
            Check_Status();
        }
    }

    private void Check_Input()
    {
        if (Input.GetKeyDown(KeyCode.S)||player.GetButtonDown("X")&&!Slashing)
        {
            Slashing = true;
            Weapon.GetComponent<AudioSource>().Play();
            Weapon.GetComponent<SpriteRenderer>().enabled = true;
            Weapon.GetComponent<Animator>().enabled = true;
            Weapon.GetComponent<Animator>().Play("SlashEffect" ,0 ,0);
            Weapon_Active_Time_count = 0;
        }
    }

    private void Check_Status()
    {
        if (Slashing)
        {
            if (GetComponent<Main_Character_Status_Manager>().status == MainCharacterStatus.Aimed)
            {
                ResetSlash();
            }
            Weapon_Active_Time_count += Time.deltaTime;
            if (Weapon_Active_Time_count > Weapon_Damage_Time)
            {
                Weapon.GetComponent<BoxCollider2D>().enabled = true;
            }
            if (Weapon_Active_Time_count > Weapon_Active_Time)
            {
                ResetSlash();
            }
        }
        else
        {
            ResetSlash();
        }
    }

    private void OnLoadLevel(LoadLevel L)
    {
        ResetSlash();
    }

    private void ResetSlash()
    {
        Weapon_Active_Time_count = 0;
        Slashing = false;
        Weapon.GetComponent<BoxCollider2D>().enabled = false;
        Weapon.GetComponent<SpriteRenderer>().enabled = false;
        Weapon.GetComponent<Animator>().enabled = false;
    }
}
