using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteIcon : MonoBehaviour
{
    public bool MainCharacter;
    public Sprite ControllerIcon;
    public Sprite KeyboardIcon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCharacter)
        {
            if (ControllerManager.MainCharacterJoystick!=null)
            {
                GetComponent<SpriteRenderer>().sprite = ControllerIcon;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = KeyboardIcon;
            }
        }
        else
        {
            if (ControllerManager.FairyJoystick != null)
            {
                GetComponent<SpriteRenderer>().sprite = ControllerIcon;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = KeyboardIcon;
            }
        }
    }
}
