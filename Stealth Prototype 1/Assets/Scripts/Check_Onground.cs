using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Onground : MonoBehaviour
{

    public bool onground;

    private const float detect_dis = 0.6f;
    private const float detect_offset = 0.35f;
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
        int layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1<<LayerMask.NameToLayer("Invisible_Object")| 1<< LayerMask.NameToLayer("Fairy") | 1<<LayerMask.NameToLayer("Path") | 1<<LayerMask.NameToLayer("Gem") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger");
        layermask = ~layermask;
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(transform.position.x-detect_offset, transform.position.y), Vector2.down, detect_dis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x+detect_offset, transform.position.y), Vector2.down, detect_dis, layermask);
        if (hit1.collider != null || hit2.collider!=null)
        {
            onground = true;
        }
        else
        {
            onground = false;
        }
    }
}
