using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHorizontalCollider : MonoBehaviour
{
    public bool RightCollide;
    public bool LeftCollide;

    private const float detect_dis = 0.55f;
    private const float detect_offset = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckLeft();
        CheckRight();
    }

    private void CheckRight()
    {
        int layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Fairy") | 1 << LayerMask.NameToLayer("Path") | 1 << LayerMask.NameToLayer("Gem") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1<<LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Portal");
        layermask = ~layermask;
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y+detect_offset), Vector2.right, detect_dis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y-detect_offset), Vector2.right, detect_dis, layermask);
        if (hit1.collider != null || hit2.collider != null)
        {
            RightCollide = true;
        }
        else
        {
            RightCollide = false;
        }

    }

    private void CheckLeft()
    {
        int layermask = 1 << LayerMask.NameToLayer("Main_Character") | 1 << LayerMask.NameToLayer("Invisible_Object") | 1 << LayerMask.NameToLayer("Fairy") | 1 << LayerMask.NameToLayer("Path") | 1 << LayerMask.NameToLayer("Gem") | 1 << LayerMask.NameToLayer("PlatformTotemTrigger") | 1 << LayerMask.NameToLayer("TutorialTrigger") | 1 << LayerMask.NameToLayer("Portal");
        layermask = ~layermask;
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + detect_offset), Vector2.left, detect_dis, layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - detect_offset), Vector2.left, detect_dis, layermask);
        if (hit1.collider != null || hit2.collider != null)
        {
            LeftCollide = true;
        }
        else
        {
            LeftCollide = false;
        }
    }
}
