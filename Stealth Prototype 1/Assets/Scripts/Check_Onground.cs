using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Onground : MonoBehaviour
{

    public bool onground;

    private const float detect_dis = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        check_onground();
    }

    private void check_onground()
    {
        int layermask = 1 << LayerMask.NameToLayer("Character");
        layermask = ~layermask;
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), transform.localScale, 0, Vector2.down, detect_dis, layermask);
        if (hit.collider != null)
        {

            onground = true;
        }
        else
        {
            onground = false;
        }
    }
}
