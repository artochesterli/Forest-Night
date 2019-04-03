using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject ob = collision.collider.gameObject;
        if (ob.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

}
