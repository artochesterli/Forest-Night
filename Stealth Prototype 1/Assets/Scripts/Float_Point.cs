using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float_Point : MonoBehaviour
{
    public bool Is_Float_Point;

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
            Is_Float_Point = !Is_Float_Point;
        }
    }

    private void Check_Float_Point()
    {
        var invisible_ward = transform.Find("Invisible_Ward").GetComponent<Invisible_Ward>();
        if (Is_Float_Point)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            invisible_ward.have_ward = false;

            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1,current_color.a);
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
            invisible_ward.have_ward = true;

            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, current_color.a);
        }
    }
}
