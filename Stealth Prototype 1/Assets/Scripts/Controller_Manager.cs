using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Controller_Manager : MonoBehaviour
{
    Joystick MainCharacterJoystick;
    Joystick FairyJoystick;
    // Start is called before the first frame update

    void Start()
    {
        MainCharacterJoystick = null;
        FairyJoystick = null;
        ReInput.ControllerConnectedEvent += OnControllerConnect;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnect;

        foreach (Joystick j in ReInput.controllers.Joysticks)
        {
            if (MainCharacterJoystick == null)
            {
                MainCharacterJoystick = j;
                GameObject.Find("Main_Character").GetComponent<PlayerId>().player.controllers.AddController(MainCharacterJoystick, false);
            }
            else if (FairyJoystick == null)
            {
                FairyJoystick = j;
                GameObject.Find("Fairy").GetComponent<PlayerId>().player.controllers.AddController(FairyJoystick, false);
            }
        }
    }

    private void OnDestroy()
    {
        ReInput.ControllerConnectedEvent -= OnControllerConnect;
        ReInput.ControllerDisconnectedEvent -= OnControllerDisconnect;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnControllerConnect(ControllerStatusChangedEventArgs args)
    {
        if (MainCharacterJoystick == null)
        {
            MainCharacterJoystick = ReInput.controllers.GetController<Joystick>(args.controllerId);
            Character_Manager.Main_Character.GetComponent<PlayerId>().player.controllers.AddController(MainCharacterJoystick, false);
        }
        else if (FairyJoystick == null)
        {
            FairyJoystick = ReInput.controllers.GetController<Joystick>(args.controllerId);
            Character_Manager.Fairy.GetComponent<PlayerId>().player.controllers.AddController(FairyJoystick, false);
        }
    }

    private void OnControllerDisconnect(ControllerStatusChangedEventArgs args)
    {
        if (MainCharacterJoystick != null && MainCharacterJoystick.id == args.controllerId)
        {
            MainCharacterJoystick = null;
        }
        else if(FairyJoystick!=null && FairyJoystick.id == args.controllerId)
        {
            FairyJoystick = null;
        }
    }
}
