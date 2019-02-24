using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Status_Manager : MonoBehaviour
{
    public int Status;

    public int PATROL = 0;
    public int ATTENTION_DRAWN = 1;
    public int ALERT = 2;
    public int AIM_CHARACTER = 3;
    public int SHOOT_CHARACTER = 4;
    public int ALERT_RELEASE = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
