using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorialTrigger : MonoBehaviour
{
    public bool MainCharacter;
    public GameObject Icon;
    public Vector2 Offset;

    private bool CharacterIn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterIn)
        {
            Icon.GetComponent<SpriteRenderer>().enabled = true;
            if (MainCharacter)
            {
                Icon.transform.position = Character_Manager.Main_Character.transform.position + (Vector3)Offset;
            }
            else
            {
                Icon.transform.position = Character_Manager.Fairy.transform.position + (Vector3)Offset;
            }
        }
        else
        {
            Icon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Main_Character") || ob.CompareTag("Fairy") && !MainCharacter)
        {
            CharacterIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Main_Character") && MainCharacter || ob.CompareTag("Fairy") && !MainCharacter)
        {
            CharacterIn = false;
        }
    }
}
