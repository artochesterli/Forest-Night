using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    public float Speed;
    public Vector2 direction;
    public GameObject target;
    public bool deadly_to_enemy;
    // Start is called before the first frame update
    void Start()
    {
        deadly_to_enemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            direction = target.transform.position - transform.position;
            direction.Normalize();
        }
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg, Vector3.forward);
    }

    public IEnumerator fly()
    {
        while (true)
        {
            if (target != null)
            {
                direction = target.transform.position - transform.position;
                direction.Normalize();
            }
            transform.position += (Vector3)(Speed * direction * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;
        if (ob.CompareTag("Main_Character") || ob.CompareTag("Fairy"))
        {
            Destroy(ob);
        }
        if (ob.CompareTag("Enemy") && deadly_to_enemy)
        {
            Destroy(ob);
        }
        if (!ob.CompareTag("Mirror_Tolem")&&!(ob.CompareTag("Enemy")&&!deadly_to_enemy))
        {
            Destroy(gameObject);
        }
    }
}
