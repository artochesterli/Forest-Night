using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject AllEnemy;
    public GameObject AllLevelMechanics;

    private Vector3 MainCharacterPos;
    private Vector3 FairyPos;
    private GameObject AllEnemyCopy;
    private GameObject AllLevelMechanicsCopy;
    // Start is called before the first frame update
    void Start()
    {
        SaveLevel();
        EventManager.instance.AddHandler<MemoryActivate>(OnMemoryActivate);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<MemoryActivate>(OnMemoryActivate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadLevel();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLevel();
        }
    }

    private void SaveLevel()
    {
        Destroy(AllEnemyCopy);
        Destroy(AllLevelMechanicsCopy);
        AllEnemyCopy = Instantiate(AllEnemy, Vector3.zero, Quaternion.Euler(0,0,0));
        AllLevelMechanicsCopy = Instantiate(AllLevelMechanics, Vector3.zero, Quaternion.Euler(0, 0, 0));
        AllEnemyCopy.transform.parent = transform;
        AllLevelMechanicsCopy.transform.parent = transform;
        AllEnemyCopy.SetActive(false);
        AllLevelMechanicsCopy.SetActive(false);
        MainCharacterPos = GameObject.Find("Main_Character").transform.position;
        FairyPos = GameObject.Find("Fairy").transform.position;
    }

    private void LoadLevel()
    {
        Character_Manager.Main_Character.transform.position = MainCharacterPos;
        Character_Manager.Fairy.transform.position = FairyPos;
        Character_Manager.Main_Character.SetActive(true);
        Character_Manager.Fairy.SetActive(true);

        List<GameObject> EnemyDestroylist=new List<GameObject>();
        List<GameObject> LevelMechanicsDestroylist = new List<GameObject>();

        foreach (Transform child in AllEnemy.transform)
        {
            EnemyDestroylist.Add(child.gameObject);
        }
        foreach(Transform child in AllLevelMechanics.transform)
        {
            LevelMechanicsDestroylist.Add(child.gameObject);
        }
        GameObject TempAllEnemy= Instantiate(AllEnemyCopy, Vector3.zero, Quaternion.Euler(0, 0, 0));
        GameObject TempAllLevelMechanics= Instantiate(AllLevelMechanicsCopy, Vector3.zero, Quaternion.Euler(0, 0, 0));

        List<GameObject> CopyEnemyList = new List<GameObject>();
        List<GameObject> CopyLevelMechanicsList = new List<GameObject>();

        foreach(Transform child in TempAllEnemy.transform)
        {
            CopyEnemyList.Add(child.gameObject);
        }
        foreach(Transform child in TempAllLevelMechanics.transform)
        {
            CopyLevelMechanicsList.Add(child.gameObject);
        }

        foreach(GameObject g in CopyEnemyList)
        {
            g.transform.parent = AllEnemy.transform;
            g.SetActive(true);
        }
        foreach(GameObject g in CopyLevelMechanicsList)
        {
            g.transform.parent = AllLevelMechanics.transform;
            g.SetActive(true);
        }
        foreach(GameObject g in EnemyDestroylist)
        {
            Destroy(g);
        }
        foreach(GameObject g in LevelMechanicsDestroylist)
        {
            Destroy(g);
        }

        Destroy(TempAllEnemy);
        Destroy(TempAllLevelMechanics);
    }

    private void OnMemoryActivate(MemoryActivate M)
    {
        SaveLevel();
    }


    private void OnCharacterDied()
    {

    }
}
