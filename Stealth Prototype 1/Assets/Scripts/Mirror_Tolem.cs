using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror_Tolem : MonoBehaviour
{
    public GameObject connected_totem;
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

        if (ob.name == "Weapon")
        {
            var self_status = GetComponent<Totem_Status_Manager>();
            self_status.Status = self_status.INGROUND;
            if (connected_totem != null)
            {
                var connected_status=connected_totem.GetComponent<Totem_Status_Manager>();
                connected_status.Status = connected_status.APPEAR;
            }
        }

        if (ob.CompareTag("Bullet_Enemy"))
        {
            ob.GetComponent<Bullet_Enemy>().target = null;
            ob.GetComponent<Bullet_Enemy>().deadly_to_enemy = true;
            ob.GetComponent<Bullet_Enemy>().direction.x *= -1;
        }
    }


}
