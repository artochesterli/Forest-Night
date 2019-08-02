using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorStateManager : MonoBehaviour
{
    public bool Activated;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Activated)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<PolygonCollider2D>().enabled = true;
            transform.Find("Tile").GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            transform.Find("Tile").GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
