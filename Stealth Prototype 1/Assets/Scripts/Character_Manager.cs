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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Main_Character.GetComponent<Invisible>().enabled = false;
            Fairy.GetComponent<Invisible>().enabled = false;
            Fairy.GetComponent<ShootArrow>().enabled = false;
        }
        else if(scene.buildIndex == 1)
        {
            Main_Character.GetComponent<Invisible>().enabled = true;
            Fairy.GetComponent<Invisible>().enabled = true;
        }
        else if (scene.buildIndex == 2)
        {
            Fairy.GetComponent<ShootArrow>().enabled = true;
        }
    }


}
