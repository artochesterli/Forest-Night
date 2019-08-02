using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public bool RequireMainCharacter;
    public bool RequireFairy;

    private bool MainCharacterEnter;
    private bool FairyEnter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckStatus();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Main_Character"))
        {
            MainCharacterEnter = true;
        }
        if (ob.CompareTag("Fairy"))
        {
            FairyEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Main_Character"))
        {
            MainCharacterEnter = false;
        }
        if (ob.CompareTag("Fairy"))
        {
            FairyEnter = false;
        }
    }

    private void CheckStatus()
    {
        bool ok = false;
        if (RequireMainCharacter && RequireFairy)
        {
            if (MainCharacterEnter && FairyEnter)
            {
                ok = true;
            }
        }
        else if (RequireMainCharacter)
        {
            if (MainCharacterEnter)
            {
                ok = true;
            }
        }
        else if (RequireFairy)
        {
            if (FairyEnter)
            {
                ok = true;
            }
        }
        if (ok)
        {
            RequireMainCharacter = false;
            RequireFairy = false;
            EventManager.instance.Fire(new TutorialOpen(transform.parent.gameObject));
        }
    }
}
