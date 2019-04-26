using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobileAnimationManager : MonoBehaviour
{
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
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EnemyMobileMoving"))
        {
            GetComponent<Animator>().Play("EnemyMobileMoving", 0, 0);
        }
    }
}
