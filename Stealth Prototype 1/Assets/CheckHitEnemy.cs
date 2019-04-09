using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHitEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HitEnemy();
    }

    private void HitEnemy()
    {
        var CharacterMove = GetComponent<CharacterMove>();
        if (CharacterMove.HitLeftWall && CharacterMove.LeftWall.CompareTag("Enemy") || CharacterMove.HitRightWall && CharacterMove.RightWall.CompareTag("Enemy")
            ||CharacterMove.OnGround&&CharacterMove.Ground.CompareTag("Enemy") || CharacterMove.HitTop && CharacterMove.Top.CompareTag("Enemy"))
        {
            EventManager.instance.Fire(new CharacterDied(gameObject));
            Destroy(gameObject);
        }
    }
}
