using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float speed;
    public Vector2 direction;
    public bool emit;

    private GameObject collision_object;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        emit = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * (Vector3)direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (emit)
        {
            collision_object = collision.GetComponent<Collider2D>().gameObject;
            if (collision_object.CompareTag("Enemy"))
            {
                collision_object.GetComponent<Enemy_Check>().time_count = 0;
                collision_object.GetComponent<Enemy_Status_Manager>().status = collision_object.GetComponent<Enemy_Status_Manager>().STUNNED;

            }

            if (collision_object.CompareTag("Mirror"))
            {
                direction.x = -direction.x;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        

    }
}
