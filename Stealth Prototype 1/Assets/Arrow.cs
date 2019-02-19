﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private GameObject collision_object;
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
        collision_object = collision.GetComponent<Collider2D>().gameObject;
        if (collision_object.CompareTag("Enemy"))
        {
            if (transform.position.x > collision_object.transform.position.x)
            {
                collision_object.GetComponent<Enemy_Check>().Check_Object_Right = true;
            }
            else
            {
                collision_object.GetComponent<Enemy_Check>().Check_Object_Right = false;
            }
            collision_object.GetComponent<Enemy_Check>().check_object_time_count = 0;
            collision_object.GetComponent<Enemy_Status_Manager>().Status = collision_object.GetComponent<Enemy_Status_Manager>().CHECK_OBJECT;

        }

        Destroy(gameObject);

    }
}
