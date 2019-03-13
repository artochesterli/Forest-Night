using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze_Manager : MonoBehaviour
{
    public static bool freeze;

    public static Vector2 MainCharacterVelocity;
    public static Vector2 FairyVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeFreeze();
        }
    }

    public void ChangeFreeze()
    {
        freeze = !freeze;
        if (freeze)
        {
            MainCharacterVelocity = Character_Manager.Main_Character.GetComponent<Rigidbody2D>().velocity;
            FairyVelocity = Character_Manager.Fairy.GetComponent<Rigidbody2D>().velocity;
            Character_Manager.Main_Character.GetComponent<Rigidbody2D>().gravityScale = 0;
            Character_Manager.Fairy.GetComponent<Rigidbody2D>().gravityScale = 0;
            Character_Manager.Main_Character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Character_Manager.Fairy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            

            if (Character_Manager.Main_Character != null)
            {
                Character_Manager.Main_Character.GetComponent<Character_Horizontal_Movement>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Character_Jump>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Character_Climb>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Transport>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Dash_To_Fairy>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Slash>().enabled = false;
            }

            if (Character_Manager.Fairy != null)
            {
                Character_Manager.Fairy.GetComponent<Character_Horizontal_Movement>().enabled = false;
                Character_Manager.Fairy.GetComponent<Character_Jump>().enabled = false;
                Character_Manager.Fairy.GetComponent<Float>().enabled = false;
                Character_Manager.Fairy.GetComponent<Transport>().enabled = false;
                Character_Manager.Fairy.GetComponent<Float_Point>().enabled = false;
                Character_Manager.Fairy.GetComponent<ShootArrow>().enabled = false;
            }

            for (int i = 0; i < Enemy_Manager.AllEnemy.transform.childCount; i++)
            {
                Enemy_Manager.AllEnemy.transform.GetChild(i).GetComponent<Enemy_Patrol>().enabled = false;
                Enemy_Manager.AllEnemy.transform.GetChild(i).GetComponent<Enemy_Check>().enabled = false;
                Enemy_Manager.AllEnemy.transform.GetChild(i).GetComponent<Chase_Gem>().enabled = false;
            }
        }
        else
        {
            Character_Manager.Main_Character.GetComponent<Rigidbody2D>().velocity = MainCharacterVelocity;
            Character_Manager.Fairy.GetComponent<Rigidbody2D>().velocity = FairyVelocity;

            if (Character_Manager.Main_Character != null)
            {
                Character_Manager.Main_Character.GetComponent<Character_Horizontal_Movement>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Character_Jump>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Character_Climb>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Transport>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Dash_To_Fairy>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Slash>().enabled = true;
            }

            if (Character_Manager.Fairy != null)
            {
                Character_Manager.Fairy.GetComponent<Character_Horizontal_Movement>().enabled = true;
                Character_Manager.Fairy.GetComponent<Character_Jump>().enabled = true;
                Character_Manager.Fairy.GetComponent<Float>().enabled = true;
                Character_Manager.Fairy.GetComponent<Transport>().enabled = true;
                Character_Manager.Fairy.GetComponent<Float_Point>().enabled = true;
                Character_Manager.Fairy.GetComponent<ShootArrow>().enabled = true;
            }

            for (int i = 0; i < Enemy_Manager.AllEnemy.transform.childCount; i++)
            {
                Enemy_Manager.AllEnemy.transform.GetChild(i).GetComponent<Enemy_Patrol>().enabled = true;
                Enemy_Manager.AllEnemy.transform.GetChild(i).GetComponent<Enemy_Check>().enabled = true;
                Enemy_Manager.AllEnemy.transform.GetChild(i).GetComponent<Chase_Gem>().enabled = true;
            }
        }
    }
}
