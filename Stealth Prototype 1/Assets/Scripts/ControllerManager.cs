using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControllerManager : MonoBehaviour
{
    Joystick MainCharacterJoystick;
    Joystick FairyJoystick;

    public static Player MainCharacter;
    public static Player Fairy;
    // Start is called before the first frame update
    void Start()
    {
        MainCharacterJoystick = null;
        FairyJoystick = null;
        MainCharacter = ReInput.players.GetPlayer(0);
        Fairy = ReInput.players.GetPlayer(1);
        ReInput.ControllerConnectedEvent += OnControllerConnect;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnect;

        foreach (Joystick j in ReInput.controllers.Joysticks)
        {
            if (MainCharacterJoystick == null)
            {
                MainCharacterJoystick = j;
                MainCharacter.controllers.AddController(MainCharacterJoystick, false);
            }
            else if (FairyJoystick == null)
            {
                FairyJoystick = j;
                Fairy.controllers.AddController(FairyJoystick, false);
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
            MainCharacter.controllers.AddController(MainCharacterJoystick, false);
        }
        else if (FairyJoystick == null)
        {
            FairyJoystick = ReInput.controllers.GetController<Joystick>(args.controllerId);
            Fairy.controllers.AddController(FairyJoystick, false);
        }
    }

    private void OnControllerDisconnect(ControllerStatusChangedEventArgs args)
    {
        if (MainCharacterJoystick != null && MainCharacterJoystick.id == args.controllerId)
        {
            MainCharacterJoystick = null;
        }
        else if (FairyJoystick != null && FairyJoystick.id == args.controllerId)
        {
            FairyJoystick = null;
        }
    }
}
