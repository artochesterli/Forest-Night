using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Controller_Manager : MonoBehaviour
{
    List<Joystick> list = new List<Joystick>();
    // Start is called before the first frame update

    void Start()
    {
        GameObject Main_Character = GameObject.Find("Main_Character").gameObject;
        GameObject Fairy = GameObject.Find("Fairy").gameObject;
        

        foreach (Joystick j in ReInput.controllers.Joysticks)
        {
            list.Add(j);
        }
        if (list.Count > 0)
        {
            Main_Character.GetComponent<PlayerId>().player.controllers.AddController(list[0], false);
        }
        if (list.Count > 1)
        {
            Fairy.GetComponent<PlayerId>().player.controllers.AddController(list[1], false);
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
