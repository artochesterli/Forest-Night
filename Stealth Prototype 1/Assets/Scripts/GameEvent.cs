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



public class TutorialOpen : GameEvent
{

}

public class TutorialClose : GameEvent
{

}

public class MenuOpen : GameEvent
{

}

public class MenuClose : GameEvent
{

}
