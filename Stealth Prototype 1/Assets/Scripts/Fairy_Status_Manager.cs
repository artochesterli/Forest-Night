using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy_Status_Manager : MonoBehaviour
{
    public int status;

    public int NORMAL = 0;
    public int FLOAT = 1;
    public int AIM = 2;
    public int CLIMBING = 3;
    public int FLOAT_PLATFORM = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color current_color = GetComponent<SpriteRenderer>().color;
        if (status == NORMAL)
        {
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
            GetComponent<Invisible>().AbleToInvisible = true;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == FLOAT)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            transform.position += Vector3.down * GetComponent<Gravity_Data>().float_down_speed * Time.deltaTime;
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, current_color.a);
        }
        else if (status == AIM)
        {
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, current_color.a);
        }
        else if (status == CLIMBING)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
        }
        else if (status == FLOAT_PLATFORM)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<SpriteRenderer>().color = new Color(100/255f, 1, 0, current_color.a);
        }
    }
}
