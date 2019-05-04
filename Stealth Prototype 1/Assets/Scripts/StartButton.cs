using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
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
            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/GameObject/LoadingLevelData"));
            g.GetComponent<LoadingLevelData>().Scene = "Level 1";
            g.name= "LoadingLevelData";
            DontDestroyOnLoad(g);
            SceneManager.LoadScene("Loading");
        }
    }

    
}
