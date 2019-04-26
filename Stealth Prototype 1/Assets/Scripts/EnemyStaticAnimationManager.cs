using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStaticAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetAnimation()
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EnemyStaticIdle"))
        {
            GetComponent<Animator>().Play("EnemyStaticIdle", 0, 0);
        }
    }
}
