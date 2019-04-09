using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public bool emit;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (transform.parent != null)
        {
            Character_Manager.Fairy.GetComponent<Fairy_Status_Manager>().status = FairyStatus.Normal;
            Destroy(gameObject);
        }*/

        if (emit)
        {
            GameObject ob = collision.GetComponent<Collider2D>().gameObject;
            if (ob.CompareTag("Enemy"))
            {
                ob.GetComponent<Enemy_Check>().time_count = 0;
                ob.GetComponent<Enemy_Status_Manager>().status = EnemyStatus.Stunned;

            }

            if (ob.CompareTag("Mirror"))
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
