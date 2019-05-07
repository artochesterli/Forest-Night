using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStaticAnimationManager : MonoBehaviour
{
    private const float MaxPlaySpeed=1;
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
        GetComponent<Animator>().speed = MaxPlaySpeed;
        if (GetComponent<Enemy_Status_Manager>().status == EnemyStatus.Stunned)
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EnemyStaticStunned"))
            {
                GetComponent<Animator>().Play("EnemyStaticStunned", 0, 0);
            }
        }
        else
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EnemyStaticIdle"))
            {
                GetComponent<Animator>().Play("EnemyStaticIdle", 0, 0);
            }
        }
    }
}
