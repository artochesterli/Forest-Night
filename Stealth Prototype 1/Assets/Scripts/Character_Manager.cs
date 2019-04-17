using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager : MonoBehaviour
{
    public static GameObject Main_Character;
    public static GameObject Fairy;

    public float Invisible_Dis_Theshold;

    private GameObject Memory;

    // Start is called before the first frame update
    void Start()
    {
        Main_Character = GameObject.Find("Main_Character").gameObject;
        Fairy = GameObject.Find("Fairy").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInvisible();
    }

    private void CheckIfInvisible()
    {
        if (Main_Character != null && Fairy != null)
        {
            float dis = Vector2.Distance(Main_Character.transform.position, Fairy.transform.position);
            if (Main_Character.GetComponent<Invisible>().AbleToInvisible && Fairy.GetComponent<Invisible>().AbleToInvisible && dis < Invisible_Dis_Theshold)
            {
                Main_Character.GetComponent<Invisible>().invisible = true;
                Fairy.GetComponent<Invisible>().invisible = true;
            }
            else
            {
                Main_Character.GetComponent<Invisible>().invisible = false;
                Fairy.GetComponent<Invisible>().invisible = false;
            }
        }
    }



}
