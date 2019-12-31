using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    public GameObject AllEnemy;
    public GameObject AllLevelMechanics;
    public GameObject Mask;

    private Vector3 MainCharacterPos;
    private Vector3 FairyPos;
    private GameObject AllEnemyCopy;
    private GameObject AllLevelMechanicsCopy;

    //private bool Loading;

    private const float LoadLevelWaitTime = 0.5f;
    private const float LoadlLevelFadeTIme = 1;


    // Start is called before the first frame update
    void Start()
    {
        SaveLevel();
        EventManager.instance.AddHandler<SaveLevel>(OnSaveLevel);
        EventManager.instance.AddHandler<CharacterDied>(OnCharacterDied);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<SaveLevel>(OnSaveLevel);
        EventManager.instance.RemoveHandler<CharacterDied>(OnCharacterDied);
    }

    // Update is called once per frame
    void Update()
    {

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

        foreach(Transform child in AllLevelMechanicsCopy.transform)
        {
            if (child.CompareTag("Platform_Totem"))
            {
                child.GetComponent<Platform_Tolem>().DisableSelf();
            }
            else if (child.CompareTag("Mirror_Totem"))
            {
                child.GetComponent<MirrorTotem>().DisableSelf();
            }
            else if (child.CompareTag("Path_Totem"))
            {
                //child.gameObject.SetActive(false);
                child.GetComponent<Path_Totem>().DisableSelf();
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        //AllLevelMechanicsCopy.SetActive(false);
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
            if (g.CompareTag("Platform_Totem"))
            {
                g.GetComponent<Platform_Tolem>().EnableSelf();
            }
            else if (g.CompareTag("Mirror_Totem"))
            {
                g.GetComponent<MirrorTotem>().EnableSelf();
            }
            else if (g.CompareTag("Path_Totem"))
            {
                //g.SetActive(false);
                g.GetComponent<Path_Totem>().EnableSelf();
            }
            else
            {
                g.gameObject.SetActive(true);
            }
            //g.SetActive(true);
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

    private void OnSaveLevel(SaveLevel S)
    {
        SaveLevel();
    }


    private void OnCharacterDied(CharacterDied C)
    {
        StartCoroutine(PerformLoadLevel());
    }

    private IEnumerator PerformLoadLevel()
    {
        yield return new WaitForSeconds(LoadLevelWaitTime);

        float TimeCount = 0;
        while (TimeCount < LoadlLevelFadeTIme)
        {
            Mask.GetComponent<RawImage>().color = new Color(0, 0, 0, TimeCount / LoadlLevelFadeTIme);
            TimeCount += Time.deltaTime;
            yield return null;
        }
        Mask.GetComponent<RawImage>().color = Color.black;

        EventManager.instance.Fire(new LoadLevel());
        LoadLevel();
        

        TimeCount = 0;
        while (TimeCount < LoadlLevelFadeTIme)
        {
            Mask.GetComponent<RawImage>().color = new Color(0, 0, 0, 1-TimeCount / LoadlLevelFadeTIme);
            TimeCount += Time.deltaTime;
            yield return null;
        }
        Mask.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);

    }
}
