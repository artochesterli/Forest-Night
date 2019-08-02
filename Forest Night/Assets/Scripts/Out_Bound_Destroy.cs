using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Out_Bound_Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (out_bound())
        {
            Destroy(gameObject);
        }
    }

    private bool out_bound()
    {
        float bound_y = Camera.main.orthographicSize;
        float bound_x = Camera.main.orthographicSize * Camera.main.pixelWidth / Camera.main.pixelHeight;
        if (transform.position.x > bound_x || transform.position.x < -bound_x || transform.position.y > bound_y || transform.position.y < -bound_y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
