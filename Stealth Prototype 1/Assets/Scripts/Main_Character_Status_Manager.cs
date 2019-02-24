using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character_Status_Manager : MonoBehaviour
{
    public int Status;

    public int NORMAL = 0;
    public int DASHING = 1;
    public int CLIMBING = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Status == NORMAL)
        {
            GetComponent<Invisible>().AbleToInvisible = true;
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
        }
        else if(Status==DASHING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else if (Status == CLIMBING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
}
