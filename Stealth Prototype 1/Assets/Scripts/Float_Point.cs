using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float_Point : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check_Input();
        Check_Float_Point();
    }

    private void Check_Input()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            var Fairy_Status = GetComponent<Fairy_Status_Manager>();
            if (Fairy_Status.status == Fairy_Status.NORMAL)
            {
                Fairy_Status.status = Fairy_Status.FLOAT;
            }
            else
            {
                Fairy_Status.status = Fairy_Status.NORMAL;
            }
        }
    }

    private void Check_Float_Point()
    {
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();

        if (Fairy_Status.status==Fairy_Status.FLOAT)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;

            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1,current_color.a);
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;

            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(38/255f, 197/255f, 243/255f, current_color.a);
        }
    }
}
