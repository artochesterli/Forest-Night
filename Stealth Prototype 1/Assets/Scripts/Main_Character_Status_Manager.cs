using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character_Status_Manager : MonoBehaviour
{
    public int status;

    public int NORMAL = 0;
    public int DASHING = 1;
    public int CLIMBING = 2;
    public int TRANSPORTING = 3;
    public int AIMED = 4;

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
        if (status == NORMAL)
        {
            GetComponent<Invisible>().AbleToInvisible = true;
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
        }
        else if (status == DASHING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else if (status == CLIMBING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else if (status == TRANSPORTING)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
        }
        else if (status == AIMED)
        {
            GetComponent<Invisible>().AbleToInvisible = false;
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Gravity_Data>().normal_gravityScale;
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
