using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float speed;
    public Vector2 direction;

    private GameObject collision_object;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * (Vector3)direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision_object = collision.GetComponent<Collider2D>().gameObject;
        if (collision_object.CompareTag("Enemy"))
        {
            if (transform.position.x > collision_object.transform.position.x)
            {
                collision_object.GetComponent<Enemy_Check>().Attention_Drawn_Right = true;
            }
            else
            {
                collision_object.GetComponent<Enemy_Check>().Attention_Drawn_Right = false;
            }
            collision_object.GetComponent<Enemy_Check>().time_count = 0;
            collision_object.GetComponent<Enemy_Status_Manager>().Status = collision_object.GetComponent<Enemy_Status_Manager>().ATTENTION_DRAWN;

        }

        Destroy(gameObject);

    }
}
