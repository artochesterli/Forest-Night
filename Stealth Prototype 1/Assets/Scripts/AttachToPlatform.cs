using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {

        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Totem_Platform_Trigger") && GetComponent<CharacterMove>().OnGround)
        {
            GetComponent<CharacterMove>().ConnectedMovingPlatform = ob.transform.parent.gameObject;
            GetComponent<CharacterMove>().PlatformSpeed = ob.transform.parent.GetComponent<Platform_Tolem>().CurrentSpeed;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Totem_Platform_Trigger"))
        {
            GetComponent<CharacterMove>().ConnectedMovingPlatform = null;
            GetComponent<CharacterMove>().PlatformSpeed = Vector2.zero;
        }
    }
}
