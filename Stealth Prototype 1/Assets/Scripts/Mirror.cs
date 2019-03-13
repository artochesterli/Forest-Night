using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
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
        GameObject ob = collision.GetComponent<Collider2D>().gameObject;

        if (ob.CompareTag("Bullet_Enemy"))
        {
            ob.GetComponent<Bullet_Enemy>().target = null;
            ob.GetComponent<Bullet_Enemy>().deadly_to_enemy = true;
            ob.GetComponent<Bullet_Enemy>().direction.x *= -1;
        }
    }
}
