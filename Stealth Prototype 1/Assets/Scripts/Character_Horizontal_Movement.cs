using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Horizontal_Movement : MonoBehaviour
{

    public float ground_speed;
    public float air_speed;

    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        check_input();
    }

    private void check_input()
    {
        var check_onground = GetComponent<Check_Onground>();
        if (Input.GetKey(KeyCode.D))
        {
            if (check_onground.onground)
            {
                transform.position += ground_speed * Vector3.right * Time.deltaTime;
            }
            else
            {
                transform.position += air_speed * Vector3.right * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (check_onground.onground)
            {
                transform.position += ground_speed * Vector3.left * Time.deltaTime;
            }
            else
            {
                transform.position += air_speed * Vector3.right * Time.deltaTime;
            }
        }
    }
}
