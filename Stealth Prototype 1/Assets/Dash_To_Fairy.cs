using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_To_Fairy : MonoBehaviour
{

    public bool detect_float_fairy;
    public float dash_distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void lock_fairy()
    {
        GameObject fairy = GetComponent<Main_Character>().Fairy;
        float current_dis = ((Vector2)(transform.position) - (Vector2)(fairy.transform.position)).magnitude;
        if (current_dis <= dash_distance)
        {

        }
    }

}
