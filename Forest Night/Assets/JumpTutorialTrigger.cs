using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorialTrigger : MonoBehaviour
{
    public GameObject JumpIcon;
    public Vector2 Offset;

    private bool MainCharacterIn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCharacterIn)
        {
            JumpIcon.GetComponent<SpriteRenderer>().enabled = true;
            JumpIcon.transform.position = Character_Manager.Main_Character.transform.position + (Vector3)Offset;
        }
        else
        {
            JumpIcon.GetComponent<SpriteRenderer>().enabled = false;
            
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Main_Character"))
        {
            MainCharacterIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Main_Character"))
        {
            MainCharacterIn = false;
        }
    }
}
