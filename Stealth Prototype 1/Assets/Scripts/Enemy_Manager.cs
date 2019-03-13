using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    public static GameObject AllEnemy;
    // Start is called before the first frame update
    void Start()
    {
        AllEnemy = GameObject.Find("AllEnemy").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
