using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("FairyIdle");
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    private void SetBaseSprite()
    {
        FairyStatus state = GetComponent<Fairy_Status_Manager>().status;
        var CharacterMove = GetComponent<CharacterMove>();
        if(state==FairyStatus.Normal && CharacterMove.speed==Vector2.zero && CharacterMove.OnGround)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprite/CharacterSprite/FairyIdle", typeof(Sprite)) as Sprite;
        }
    }

    private void SetAnimation()
    {
        var state = GetComponent<Fairy_Status_Manager>();
        var CharacterMove = GetComponent<CharacterMove>();
        if (state.status == FairyStatus.Normal && CharacterMove.OnGround && Mathf.Abs(CharacterMove.speed.x) > 0)
        {
            GetComponent<Animator>().Play("FairyWalk");
            return;
        }
        if (state.status == FairyStatus.Normal && CharacterMove.speed == Vector2.zero && CharacterMove.OnGround)
        {
            GetComponent<Animator>().Play("FairyIdle");
            return;
        }
        
        if(state.status == FairyStatus.Normal && !CharacterMove.OnGround && CharacterMove.speed.y > 0)
        {
            
        }
    }
}
