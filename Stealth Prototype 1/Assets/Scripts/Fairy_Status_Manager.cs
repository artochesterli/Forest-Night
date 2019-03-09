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
    public int TRANSPORTING = 5;
    public int AIMED = 6;

    private float AimedTimeCount;

    private const float AimedDiedTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        set_status();
        check_aimed();
    }

    private void set_status()
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
            transform.parent = null;

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
            GetComponent<SpriteRenderer>().color = new Color(100 / 255f, 1, 0, current_color.a);
            transform.parent = null;
        }
        else if (status == TRANSPORTING)
        {
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
            
        }
        else if (status == AIMED)
        {
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<SpriteRenderer>().color = new Color(38 / 255f, 197 / 255f, 243 / 255f, current_color.a);
            transform.parent = null;
        }
    }

    private void check_aimed()
    {
        if (status == AIMED)
        {
            AimedTimeCount += Time.deltaTime;
            if (AimedTimeCount > AimedDiedTime)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            AimedTimeCount = 0;
        }
    }
}
