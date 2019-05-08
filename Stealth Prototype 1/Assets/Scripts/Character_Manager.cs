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
    private GameObject ShieldEnergy;

    private const int InvisibleUnlockLevel = 2;
    private const int ArrowUnlockLevel = 3;
    private void OnEnable()
    {
        Main_Character = GameObject.Find("Main_Character").gameObject;
        Fairy = GameObject.Find("Fairy").gameObject;
        EventManager.instance.AddHandler<EnterLevel>(OnEnterLevel);
        EventManager.instance.AddHandler<FreezeGame>(OnFreezeGame);
        EventManager.instance.AddHandler<UnFreezeGame>(OnUnFreezeGame);
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterLevel>(OnEnterLevel);
        EventManager.instance.RemoveHandler<FreezeGame>(OnFreezeGame);
        EventManager.instance.RemoveHandler<UnFreezeGame>(OnUnFreezeGame);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInvisible();
        SetShield();
    }

    private void CheckIfInvisible()
    {
        if (Main_Character.GetComponent<Invisible>().enabled && Fairy.GetComponent<Invisible>().enabled)
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
        else
        {
            Main_Character.GetComponent<Invisible>().invisible = false;
            Fairy.GetComponent<Invisible>().invisible = false;
        }
    }

    private void SetShield()
    {
        if(Main_Character.GetComponent<Invisible>().invisible&& Fairy.GetComponent<Invisible>().invisible)
        {
            if (InvisibleShield == null)
            {
                InvisibleShield = (GameObject)Instantiate(Resources.Load("Prefabs/VFX/InvisibleShield"));
                ShieldEnergy = (GameObject)Instantiate(Resources.Load("Prefabs/VFX/ShieldEnergy"));
            }
            InvisibleShield.transform.position = (Main_Character.transform.position + Fairy.transform.position) / 2;
            float MainCharacterGroundDis = Main_Character.GetComponent<CharacterMove>().OnGroundThreshold;
            float FairyGroundDis = Fairy.GetComponent<CharacterMove>().OnGroundThreshold;
            if (Main_Character.transform.position.y- MainCharacterGroundDis >  Fairy.transform.position.y - FairyGroundDis)
            {
                ShieldEnergy.transform.position = new Vector3(InvisibleShield.transform.position.x, Fairy.transform.position.y - FairyGroundDis, InvisibleShield.transform.position.z);
            }
            else
            {
                ShieldEnergy.transform.position = new Vector3(InvisibleShield.transform.position.x, Main_Character.transform.position.y - MainCharacterGroundDis, InvisibleShield.transform.position.z);
            }
        }
        else
        {
            Destroy(InvisibleShield);
            Destroy(ShieldEnergy);
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

    private void OnFreezeGame(FreezeGame F)
    {
        Main_Character.GetComponent<Animator>().enabled = false;
        Main_Character.GetComponent<MainCharacterHorizontalMovement>().enabled = false;
        Main_Character.GetComponent<Character_Jump>().enabled = false;
        Main_Character.GetComponent<Character_Climb>().enabled = false;
        Main_Character.GetComponent<Dash_To_Fairy>().enabled = false;
        Main_Character.GetComponent<Slash>().enabled = false;
        Main_Character.GetComponent<CharacterMove>().enabled = false;

        Fairy.GetComponent<Animator>().enabled = false;
        Fairy.GetComponent<FairyHorizontalMovement>().enabled = false;
        Fairy.GetComponent<Character_Jump>().enabled = false;
        Fairy.GetComponent<Float>().enabled = false;
        Fairy.GetComponent<Float_Point>().enabled = false;
        Fairy.GetComponent<ShootArrow>().enabled = false;
        Fairy.GetComponent<CharacterMove>().enabled = false;
    }

    private void OnUnFreezeGame(UnFreezeGame F)
    {
        Main_Character.GetComponent<Animator>().enabled = true;
        Main_Character.GetComponent<MainCharacterHorizontalMovement>().enabled = true;
        Main_Character.GetComponent<Character_Jump>().enabled = true;
        Main_Character.GetComponent<Character_Climb>().enabled = true;
        Main_Character.GetComponent<Dash_To_Fairy>().enabled = true;
        Main_Character.GetComponent<Slash>().enabled = true;
        Main_Character.GetComponent<CharacterMove>().enabled = true;

        Fairy.GetComponent<Animator>().enabled = true;
        Fairy.GetComponent<FairyHorizontalMovement>().enabled = true;
        Fairy.GetComponent<Character_Jump>().enabled = true;
        Fairy.GetComponent<Float>().enabled = true;
        Fairy.GetComponent<Float_Point>().enabled = true;
        if (GetComponent<Level_Manager>().LevelIndex >= ArrowUnlockLevel)
        {
            Fairy.GetComponent<ShootArrow>().enabled = true;
        }
        Fairy.GetComponent<CharacterMove>().enabled = true;
    }
}
