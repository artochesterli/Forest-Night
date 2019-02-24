using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Slash : MonoBehaviour
{

    private Player player;
    private GameObject Weapon;
    private float Weapon_Active_Time_count;
    private bool Weapon_Active;

    private const float Weapon_Active_Time=0.3f;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerId>().player;
        Weapon = transform.Find("Weapon").gameObject;
        Weapon.SetActive(false);
        Weapon_Active = true;
        Weapon_Active_Time_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Check_Input();
        Check_Status();
    }

    private void Check_Input()
    {
        if (player.GetButtonDown("X")&&!Weapon_Active)
        {
            Weapon_Active = true;
            Weapon.SetActive(true);
            Weapon_Active_Time_count = 0;
        }
    }

    private void Check_Status()
    {
        if (Weapon_Active)
        {
            Weapon_Active_Time_count += Time.deltaTime;
            if (Weapon_Active_Time_count > Weapon_Active_Time)
            {
                Weapon_Active_Time_count = 0;
                Weapon_Active = false;
                Weapon.SetActive(false);
            }
        }
    }
}
