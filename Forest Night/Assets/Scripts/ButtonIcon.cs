﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIcon : MonoBehaviour
{
    public Sprite SpriteWithController;
    public Sprite SpriteWithoutController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ControllerManager.MainCharacterJoystick != null)
        {
            GetComponent<Image>().sprite = SpriteWithController;
        }
        else
        {
            GetComponent<Image>().sprite = SpriteWithoutController;
        }
        
    }
}