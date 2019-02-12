using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Jump : MonoBehaviour
{

    public float jump_initial_velocity_y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        check_jump();
    }

    private void check_jump()
    {
        var check_onground = GetComponent<Check_Onground>();
        if (Input.GetKeyDown(KeyCode.Space)&&check_onground.onground)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(0, jump_initial_velocity_y);
        }
    }


}
