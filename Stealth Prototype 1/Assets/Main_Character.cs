using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character : MonoBehaviour
{
    public GameObject Fairy;
    // Start is called before the first frame update
    void Start()
    {
        Fairy = GameObject.Find("Fairy").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
