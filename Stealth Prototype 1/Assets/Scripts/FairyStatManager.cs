using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyStatManager : MonoBehaviour
{
    public float GroundHeight;
    public float AirHeight;

    public Vector2 GroundOffset;
    public Vector2 AirOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStat();
    }

    private void UpdateStat()
    {
        float CurrentHeight;
        Vector2 CurrentOffset;
        var CharacterMove = GetComponent<CharacterMove>();
        if (CharacterMove.OnGround)
        {
            CurrentHeight = GroundHeight;
            CurrentOffset = GroundOffset;
        }
        else
        {
            CurrentHeight = AirHeight;
            CurrentOffset = AirOffset;
        }
        CharacterMove.HitWallDetectOffset = CurrentHeight / 2;
        CharacterMove.BodyOffset = CurrentOffset;
        //CharacterMove.HitTopThreshold = CurrentHeight / 2;
    }
}
