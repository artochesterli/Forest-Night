using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Status_Manager : MonoBehaviour
{
    public int status;

    public int PATROL = 0;
    public int ATTENTION_DRAWN = 1;
    public int ALERT = 2;
    public int SHOOT_CHARACTER = 4;
    public int ALERT_RELEASE = 5;
    public int DRAWN_BY_GEM = 6;
    public int STUNNED = 7;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        set_status();
    }

    private void set_status()
    {
        GameObject view = transform.Find("View").gameObject;
        if (status == SHOOT_CHARACTER || status==STUNNED)
        {
            view.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            view.GetComponent<SpriteRenderer>().enabled = true;
        }
    }


}
