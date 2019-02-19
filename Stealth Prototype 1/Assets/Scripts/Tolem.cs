using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tolem : MonoBehaviour
{
    public GameObject connected_ground;
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
        if (collision.GetComponent<Collider2D>().gameObject.name == "Weapon")
        {
            Restore_land();
            Destroy(gameObject);
        }
    }

    private void Restore_land()
    {
        if (connected_ground != null)
        {
            int number = connected_ground.transform.childCount;
            for(int i = 0; i < number; i++)
            {
                Instantiate(Resources.Load("Prefabs/Normal Tile"), connected_ground.transform.GetChild(i).position, new Quaternion(0, 0, 0, 0));
            }
            for(int i = 0; i < number; i++)
            {
                Destroy(connected_ground.transform.GetChild(i).gameObject);
            }
        }
    }
}
