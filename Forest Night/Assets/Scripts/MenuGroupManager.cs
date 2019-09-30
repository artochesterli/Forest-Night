using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGroupManager : MonoBehaviour
{
    public static GameObject CurrentActivatedMenu;
    public static GameObject CurrentSelectedButton;

    public GameObject StartMenu;
    public GameObject StartButton;

    // Start is called before the first frame update
    void Start()
    {
        CurrentActivatedMenu = StartMenu;
        CurrentSelectedButton = StartButton;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (CurrentActivatedMenu!=null && CurrentSelectedButton != null)
        {
            
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (InputClick())
        {
            if (CurrentSelectedButton != null)
            {
                if (CurrentSelectedButton.GetComponent<ForwardButton>() != null)
                {
                    EventManager.instance.Fire(new ExitMenu(CurrentActivatedMenu));
                    EventManager.instance.Fire(new EnterMenu(CurrentSelectedButton.GetComponent<ForwardButton>().LinkedMenu));
                    GetComponent<AudioSource>().Play();
                }
                else if(CurrentSelectedButton.GetComponent<FunctionButton>()!=null)
                {
                    EventManager.instance.Fire(new ButtonClicked(CurrentSelectedButton));
                    GetComponent<AudioSource>().Play();
                }
            }

            
        }
        else if (InputBack())
        {
            if (CurrentActivatedMenu.GetComponent<ChildMenu>() != null)
            {
                EventManager.instance.Fire(new ExitMenu(CurrentActivatedMenu));
                EventManager.instance.Fire(new EnterMenu(CurrentActivatedMenu.GetComponent<ChildMenu>().ParentMenu));
            }
        }
    }

    private bool InputClick()
    {
        return ControllerManager.MainCharacterJoystick != null && ControllerManager.MainCharacter.GetButtonDown("A") 
            || ControllerManager.FairyJoystick != null && ControllerManager.Fairy.GetButtonDown("A")
            || Input.GetKeyDown(KeyCode.Return);

        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("A");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Return);
        }
    }

    private bool InputBack()
    {
        return ControllerManager.MainCharacterJoystick != null&& ControllerManager.MainCharacter.GetButtonDown("B")
            || ControllerManager.FairyJoystick != null && ControllerManager.Fairy.GetButtonDown("B")
            || Input.GetKeyDown(KeyCode.Backspace);

        if (ControllerManager.MainCharacterJoystick != null)
        {
            return ControllerManager.MainCharacter.GetButtonDown("B");
        }
        else
        {
            return Input.GetKeyDown(KeyCode.Backspace);
        }
    }
}
