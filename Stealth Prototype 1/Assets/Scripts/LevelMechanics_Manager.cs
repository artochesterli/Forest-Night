using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMechanics_Manager : MonoBehaviour
{
    public static GameObject AllLevelMechanics;
    // Start is called before the first frame update
    void Start()
    {
        AllLevelMechanics = GameObject.Find("AllLevelMechanics").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
