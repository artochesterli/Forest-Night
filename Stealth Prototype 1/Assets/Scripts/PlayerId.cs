using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerId : MonoBehaviour
{
    public int Id;
    public Player player;
    // Start is called before the first frame update
    private void Awake()
    {
        player = ReInput.players.GetPlayer(Id);
    }
    void Start()
    {

        //ReInput.ControllerConnectedEvent += OnControllerConnected;

        // Assign each Joystick to a Player initially

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick) return; // skip if this isn't a Joystick

        // Assign Joystick to first Player that doesn't have any assigned
        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
    }

    void AssignJoystickToNextOpenPlayer(Joystick j)
    {
        foreach (Player p in ReInput.players.Players)
        {
            if (p.controllers.joystickCount > 0) continue; // player already has a joystick
            p.controllers.AddController(j, true); // assign joystick to player
            return;
        }
    }
}
