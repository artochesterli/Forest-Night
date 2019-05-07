using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Freeze_Manager : MonoBehaviour
{
    public static bool freeze;

    public static bool ShowTutorial;
    public static bool ShowMenu;

    public static Vector2 MainCharacterVelocity;
    public static Vector2 FairyVelocity;

    public GameObject AllEnemy;
    public GameObject AllLevelMechanics;
    // Start is called before the first frame update
    void Start()
    {
        
        EventManager.instance.AddHandler<TutorialOpen>(OnTutorialOpen);
        EventManager.instance.AddHandler<TutorialClose>(OnTutorialClose);
        EventManager.instance.AddHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.AddHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<TutorialOpen>(OnTutorialOpen);
        EventManager.instance.RemoveHandler<TutorialClose>(OnTutorialClose);
        EventManager.instance.RemoveHandler<GameSceneMenuOpen>(OnGameSceneMenuOpen);
        EventManager.instance.RemoveHandler<GameSceneMenuClose>(OnGameSceneMenuClose);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeFreeze()
    {
        freeze = !freeze;
        if (freeze)
        {
            if (Character_Manager.Main_Character != null)
            {
                Character_Manager.Main_Character.GetComponent<Animator>().enabled = false;
                Character_Manager.Main_Character.GetComponent<MainCharacterHorizontalMovement>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Character_Jump>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Character_Climb>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Dash_To_Fairy>().enabled = false;
                Character_Manager.Main_Character.GetComponent<Slash>().enabled = false;
                Character_Manager.Main_Character.GetComponent<CharacterMove>().enabled = false;
            }

            if (Character_Manager.Fairy != null)
            {
                Character_Manager.Fairy.GetComponent<Animator>().enabled = false;
                Character_Manager.Fairy.GetComponent<FairyHorizontalMovement>().enabled = false;
                Character_Manager.Fairy.GetComponent<Character_Jump>().enabled = false;
                Character_Manager.Fairy.GetComponent<Float>().enabled = false;
                Character_Manager.Fairy.GetComponent<Float_Point>().enabled = false;
                Character_Manager.Fairy.GetComponent<ShootArrow>().enabled = false;
                Character_Manager.Fairy.GetComponent<CharacterMove>().enabled = false;
            }

            for (int i = 0; i < AllEnemy.transform.childCount; i++)
            {
                AllEnemy.transform.GetChild(i).GetComponent<Animator>().enabled = false;
                AllEnemy.transform.GetChild(i).GetComponent<Enemy_Patrol>().enabled = false;
                AllEnemy.transform.GetChild(i).GetComponent<Enemy_Check>().enabled = false;
            }

            for(int i = 0; i < AllLevelMechanics.transform.childCount; i++)
            {
                if (AllLevelMechanics.transform.GetChild(i).CompareTag("Platform_Totem"))
                {
                    AllLevelMechanics.transform.GetChild(i).GetComponent<Platform_Tolem>().enabled = false;
                }
                else if (AllLevelMechanics.transform.GetChild(i).CompareTag("Mirror_Totem"))
                {
                    AllLevelMechanics.transform.GetChild(i).GetComponent<MirrorTotem>().enabled = false;
                }
                else if (AllLevelMechanics.transform.GetChild(i).CompareTag("Path_Totem"))
                {
                    AllLevelMechanics.transform.GetChild(i).GetComponent<Path_Totem>().enabled = false;
                }
            }

        }
        else
        {

            if (Character_Manager.Main_Character != null)
            {
                Character_Manager.Main_Character.GetComponent<Animator>().enabled = true;
                Character_Manager.Main_Character.GetComponent<MainCharacterHorizontalMovement>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Character_Jump>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Character_Climb>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Dash_To_Fairy>().enabled = true;
                Character_Manager.Main_Character.GetComponent<Slash>().enabled = true;
                Character_Manager.Main_Character.GetComponent<CharacterMove>().enabled = true;
            }

            if (Character_Manager.Fairy != null)
            {
                Character_Manager.Fairy.GetComponent<Animator>().enabled = true;
                Character_Manager.Fairy.GetComponent<FairyHorizontalMovement>().enabled = true;
                Character_Manager.Fairy.GetComponent<Character_Jump>().enabled = true;
                Character_Manager.Fairy.GetComponent<Float>().enabled = true;
                Character_Manager.Fairy.GetComponent<Float_Point>().enabled = true;
                if (GetComponent<Level_Manager>().LevelIndex > 2)
                {
                    Character_Manager.Fairy.GetComponent<ShootArrow>().enabled = true;
                }
                Character_Manager.Fairy.GetComponent<CharacterMove>().enabled = true;
            }

            for (int i = 0; i < AllEnemy.transform.childCount; i++)
            {
                AllEnemy.transform.GetChild(i).GetComponent<Animator>().enabled = true;
                AllEnemy.transform.GetChild(i).GetComponent<Enemy_Patrol>().enabled = true;
                AllEnemy.transform.GetChild(i).GetComponent<Enemy_Check>().enabled = true;
            }

            for (int i = 0; i < AllLevelMechanics.transform.childCount; i++)
            {
                if (AllLevelMechanics.transform.GetChild(i).CompareTag("Platform_Totem"))
                {
                    AllLevelMechanics.transform.GetChild(i).GetComponent<Platform_Tolem>().enabled = true;
                }
                else if (AllLevelMechanics.transform.GetChild(i).CompareTag("Mirror_Totem"))
                {
                    AllLevelMechanics.transform.GetChild(i).GetComponent<MirrorTotem>().enabled = true;
                }
                else if (AllLevelMechanics.transform.GetChild(i).CompareTag("Path_Totem"))
                {
                    AllLevelMechanics.transform.GetChild(i).GetComponent<Path_Totem>().enabled = true;
                }
            }
        }
    }

    private void OnTutorialOpen(TutorialOpen T)
    {
        ShowTutorial = true;
        ChangeFreeze();
    }

    private void OnGameSceneMenuOpen(GameSceneMenuOpen M)
    {
        ShowMenu = true;
        if (!ShowTutorial)
        {
            ChangeFreeze();
        }
    }

    private void OnTutorialClose(TutorialClose T)
    {
        ShowTutorial = false;
        ChangeFreeze();
    }

    private void OnGameSceneMenuClose(GameSceneMenuClose M)
    {
        ShowMenu = false;
        if (!ShowTutorial)
        {
            ChangeFreeze();
        }
    }
}
