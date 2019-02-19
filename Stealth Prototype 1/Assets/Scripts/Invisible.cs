using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool invisible;
    public float invisible_alpha;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (invisible)
        {
            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(current_color.r, current_color.g, current_color.b, invisible_alpha);
            gameObject.layer = LayerMask.NameToLayer("Invisible_Object");
        }
        else
        {
            Color current_color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(current_color.r, current_color.g, current_color.b, 1);
            if (GetComponent<PlayerId>().Id == 0)
            {
                gameObject.layer = LayerMask.NameToLayer("Main_Character");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Fairy");
            }
        }
    }
}
