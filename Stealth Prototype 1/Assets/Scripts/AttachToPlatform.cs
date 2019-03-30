using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Totem_Platform_Trigger") && GetComponent<CharacterMove>().OnGround)
        {
            GetComponent<CharacterMove>().ConnectedMovingPlatform = ob.transform.parent.gameObject;
            //transform.parent = ob.transform.parent;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Totem_Platform_Trigger"))
        {
            GetComponent<CharacterMove>().ConnectedMovingPlatform = null;
            //transform.parent = null;
        }
    }
}
