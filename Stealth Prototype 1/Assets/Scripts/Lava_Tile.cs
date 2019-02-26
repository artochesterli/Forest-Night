using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava_Tile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject ob = collision.collider.gameObject;
        if (ob.CompareTag("Main_Character") || ob.CompareTag("Fairy") || ob.CompareTag("Enemy"))
        {
            Destroy(ob);
        }
    }
}
