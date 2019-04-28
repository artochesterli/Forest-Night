using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobileAnimationManager : MonoBehaviour
{
    private const float WalkingPlaySpeed = 1;
    private const float StopPlaySpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        if(GetComponent<Enemy_Status_Manager>().status==EnemyStatus.Patrol && !GetComponent<Enemy_Patrol>().IsObserving)
        {
            GetComponent<Animator>().speed = WalkingPlaySpeed;
        }
        else
        {
            GetComponent<Animator>().speed = StopPlaySpeed;
        }
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EnemyMobileMoving"))
        {
            GetComponent<Animator>().Play("EnemyMobileMoving", 0, 0);
        }
    }
}
