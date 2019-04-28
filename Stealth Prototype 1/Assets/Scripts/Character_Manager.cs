using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character_Manager : MonoBehaviour
{
    public static GameObject Main_Character;
    public static GameObject Fairy;

    public float Invisible_Dis_Theshold;

    private GameObject Memory;

    private void OnEnable()
    {
        Main_Character = GameObject.Find("Main_Character").gameObject;
        Fairy = GameObject.Find("Fairy").gameObject;
        EventManager.instance.AddHandler<EnterLevel>(OnEnterLevel);
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterLevel>(OnEnterLevel);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInvisible();
    }

    private void CheckIfInvisible()
    {
        if (Main_Character != null && Fairy != null && Main_Character.GetComponent<Invisible>()!=null && Fairy.GetComponent<Invisible>()!=null)
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

    private void OnEnterLevel(EnterLevel E)
    {
        if (E.Level == 1)
        {
            Main_Character.GetComponent<Invisible>().enabled = false;
            Fairy.GetComponent<Invisible>().enabled = false;
            Fairy.GetComponent<ShootArrow>().enabled = false;
        }
        else if(E.Level == 2)
        {
            Fairy.GetComponent<ShootArrow>().enabled = false;
        }
    }

}
