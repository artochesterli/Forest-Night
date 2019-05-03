using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyTutorialTrigger : MonoBehaviour
{
    private bool MainCharacterIn;

    public GameObject SlashIcon;
    public GameObject ConnectedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCharacterIn && ConnectedEnemy!=null)
        {
            SlashIcon.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            SlashIcon.GetComponent<SpriteRenderer>().enabled = false;
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
