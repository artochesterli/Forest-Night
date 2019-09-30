using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Freeze_Manager : MonoBehaviour
{
    public static bool Frozen;
    public static bool ShowTutorial;

    public GameObject AllEnemy;
    public GameObject AllLevelMechanics;
    // Start is called before the first frame update
    void Start()
    {
        Frozen = false;
        ShowTutorial = false;
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

    private void Freeze()
    {
        Frozen = true;
        for (int i = 0; i < AllEnemy.transform.childCount; i++)
        {
            AllEnemy.transform.GetChild(i).GetComponent<Animator>().enabled = false;
            AllEnemy.transform.GetChild(i).GetComponent<Enemy_Patrol>().enabled = false;
            AllEnemy.transform.GetChild(i).GetComponent<Enemy_Check>().enabled = false;
        }

        EventManager.instance.Fire(new FreezeGame(GetComponent<Level_Manager>().LevelIndex));
    }

    private void UnFreeze()
    {
        Frozen = false;
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
        EventManager.instance.Fire(new UnFreezeGame(GetComponent<Level_Manager>().LevelIndex));
    }

    private void OnTutorialOpen(TutorialOpen T)
    {
        ShowTutorial = true;
        Freeze();
    }

    private void OnGameSceneMenuOpen(GameSceneMenuOpen M)
    {
        Freeze();
    }

    private void OnTutorialClose(TutorialClose T)
    {
        ShowTutorial = false;
        UnFreeze();
    }

    private void OnGameSceneMenuClose(GameSceneMenuClose M)
    {
        if (!ShowTutorial)
        {
            UnFreeze();
        }
    }
}
