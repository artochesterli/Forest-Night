using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collision_object = collision.GetComponent<Collider2D>().gameObject;
        if (collision_object.CompareTag("Enemy"))
        {
            if (transform.position.x > collision_object.transform.position.x)
            {
                collision_object.GetComponent<Enemy_Check>().Check_Right = true;
            }
            else
            {
                collision_object.GetComponent<Enemy_Check>().Check_Right = false;
            }
            StartCoroutine(collision_object.GetComponent<Enemy_Check>().Check_Object());
        }
        Destroy(gameObject);
    }
}
