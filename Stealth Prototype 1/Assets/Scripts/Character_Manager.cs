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
    private GameObject InvisibleShield;

    private const int InvisibleUnlockLevel = 2;
    private const int ArrowUnlockLevel = 3;
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
        SetShield();
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

    private void SetShield()
    {
        if(Main_Character.GetComponent<Invisible>().invisible&& Fairy.GetComponent<Invisible>().invisible)
        {
            if (InvisibleShield == null)
            {
                InvisibleShield = (GameObject)Instantiate(Resources.Load("Prefabs/VFX/InvisibleShield"));
            }
            InvisibleShield.transform.position = (Main_Character.transform.position + Fairy.transform.position) / 2;
        }
        else
        {
            Destroy(InvisibleShield);
        }
    }

    private void OnEnterLevel(EnterLevel E)
    {
        if (E.Level < InvisibleUnlockLevel)
        {
            Main_Character.GetComponent<Invisible>().enabled = false;
            Fairy.GetComponent<Invisible>().enabled = false;
        }
        if(E.Level < ArrowUnlockLevel)
        {
            Fairy.GetComponent<ShootArrow>().enabled = false;
        }
    }

}
