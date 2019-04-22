using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent { }

public class CharacterDied : GameEvent
{
    public GameObject DeadCharacter;
    public CharacterDied(GameObject C)
    {
        DeadCharacter = C;
    }
}

public class CharacterHitSpineEdge : GameEvent
{
    public GameObject Character;
    public CharacterHitSpineEdge(GameObject C)
    {
        Character = C;
    }
}

public class SaveLevel : GameEvent { }

public class LoadLevel : GameEvent { }

public class MemoryActivate : GameEvent { }

public class EnterControlMenu : GameEvent { }
public class ExitControlMenu : GameEvent { }

public class EnterObjectsMenu : GameEvent { }
public class ExitObjectsMenu : GameEvent { }

public class EnterAbilitiesMenu : GameEvent { }
public class ExitAbilitiesMenu : GameEvent { }

public class EnterMainHelpMenu : GameEvent { }
public class ExitMainHelpMenu : GameEvent { }

public class EnterMainMenu : GameEvent { }
public class ExitMainMenu : GameEvent { }



public class ButtonClicked : GameEvent
{
    public GameObject Button;
    public ButtonClicked(GameObject B)
    {
        Button = B;
    }
}

public class TutorialOpen : GameEvent
{
    public GameObject Tutorial;
    public TutorialOpen(GameObject T)
    {
        Tutorial = T;
    }
}

public class TutorialClose : GameEvent
{
    public GameObject Tutorial;
    public TutorialClose(GameObject T)
    {
        Tutorial = T;
    }
}

public class MenuOpen : GameEvent
{

}

public class MenuClose : GameEvent
{

}
