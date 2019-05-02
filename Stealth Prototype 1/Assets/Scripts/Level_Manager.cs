using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    public int LevelIndex;
    // Start is called before the first frame update

    private void OnEnable()
    {
        EventManager.instance.Fire(new EnterLevel(LevelIndex));
    }
}
