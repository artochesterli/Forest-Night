using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.AddHandler<ButtonClicked>(OnButtonClicked);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<ButtonClicked>(OnButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClicked(ButtonClicked Click)
    {
        if (Click.Button == gameObject)
        {
            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/System/LoadingLevelData"));
            string name = "Level " + SaveDataManager.data.Progress[SaveDataManager.data.CurrentSaveSlot].ToString();
            g.GetComponent<LoadingLevelData>().Scene = name;
            g.name = "LoadingLevelData";
            DontDestroyOnLoad(g);
            SceneManager.LoadScene("Loading");
        }
    }

}
