using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlot : MonoBehaviour
{
    public int Index;
    public int Level;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<ButtonClicked>(OnButtonClicked);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ButtonClicked>(OnButtonClicked);
    }

    private void OnButtonClicked(ButtonClicked B)
    {
        if (B.Button == gameObject)
        {
            SaveDataManager.data.CurrentSaveSlot = Index;
            //SaveDataManager.SaveData();
            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/GameObject/LoadingLevelData"));
            string name;
            if (Level > 0)
            {
                name= "Level " + Level;
            }
            else
            {
                name = "Level 1";
            }
            g.GetComponent<LoadingLevelData>().Scene = name;
            g.name = "LoadingLevelData";
            DontDestroyOnLoad(g);
            SceneManager.LoadScene("Loading");
        }
    }
}
