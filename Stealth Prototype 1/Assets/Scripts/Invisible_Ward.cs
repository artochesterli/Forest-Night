using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible_Ward : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check_Ward();
    }

    private void Check_Ward()
    {
        var Fairy_Status = transform.parent.GetComponent<Fairy_Status_Manager>();
        if (Fairy_Status.status == Fairy_Status.NORMAL)
        {
            transform.parent.GetComponent<Invisible>().invisible = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            transform.parent.GetComponent<Invisible>().invisible = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collided_object = collision.GetComponent<Collider2D>().gameObject;
        if (collided_object.CompareTag("Main_Character")||collided_object.CompareTag("Fairy"))

        {
            collided_object.GetComponent<Invisible>().invisible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collided_object = collision.GetComponent<Collider2D>().gameObject;
        if (collided_object.CompareTag("Main_Character") || collided_object.CompareTag("Fairy"))
        {
            collided_object.GetComponent<Invisible>().invisible = false;
        }
    }
}
