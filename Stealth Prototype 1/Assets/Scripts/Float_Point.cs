using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Float_Point : MonoBehaviour
{
    public Vector2 DashOffset;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = ControllerManager.Fairy;
        EventManager.instance.AddHandler<LoadLevel>(OnLoadLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<LoadLevel>(OnLoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (Fairy_Status.status != FairyStatus.Aimed && Fairy_Status.status!=FairyStatus.KnockBack && Fairy_Status.status != FairyStatus.Climbing && !GetComponent<CharacterMove>().OnGround)
        {
            Check_Input();
        }
        else
        {
            DeactivateFieldParticle();
            if (Fairy_Status.status == FairyStatus.FloatPlatform)
            {
                Fairy_Status.status = FairyStatus.Normal;
            }
        }
    }

    private void Check_Input()
    {
        var Fairy_Status = GetComponent<Fairy_Status_Manager>();
        if (player.GetButton("LT"))
        {
            if(Fairy_Status.status != FairyStatus.FloatPlatform)
            {
                ActivateFieldParticle();
            }
            Fairy_Status.status = FairyStatus.FloatPlatform;
            
        }
        else
        {
            DeactivateFieldParticle();
            if (Fairy_Status.status == FairyStatus.FloatPlatform)
            {
                Fairy_Status.status = FairyStatus.Normal;
            }
        }
    }

    private void ActivateFieldParticle()
    {
        GameObject FairyField = transform.Find("FairyField").gameObject;
        foreach(Transform child in FairyField.transform)
        {
            child.GetComponent<ParticleSystem>().Play();
        }
    }

    private void DeactivateFieldParticle()
    {
        GameObject FairyField = transform.Find("FairyField").gameObject;
        foreach (Transform child in FairyField.transform)
        {
            child.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void OnLoadLevel(LoadLevel L)
    {
        DeactivateFieldParticle();
    }
}
