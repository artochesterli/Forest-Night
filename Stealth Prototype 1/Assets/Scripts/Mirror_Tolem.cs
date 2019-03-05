using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror_Tolem : MonoBehaviour
{
    public List<GameObject> connected_mirrors;
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
            for (int i = 0; i < connected_mirrors.Count; i++)
            {
                var connected_status = connected_mirrors[i].GetComponent<Totem_Status_Manager>();
                if (connected_status.Status == connected_status.APPEAR)
                {
                    connected_status.Status = connected_status.INGROUND;
                }
                else if(connected_status.Status == connected_status.INGROUND)
                {
                    connected_status.Status = connected_status.APPEAR;
                }
                
            }
        }

        
    }


}
