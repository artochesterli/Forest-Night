using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager : MonoBehaviour
{
    public static GameObject Main_Character;
    public static GameObject Fairy;
    private void Awake()
    {
        Main_Character = GameObject.Find("Main_Character").gameObject;
        Fairy = GameObject.Find("Fairy").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
