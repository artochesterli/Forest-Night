using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem_Status_Manager : MonoBehaviour
{
    public int Status;

    public int APPEAR = 0;
    public int INGROUND = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Status == APPEAR)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (Status == INGROUND)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
