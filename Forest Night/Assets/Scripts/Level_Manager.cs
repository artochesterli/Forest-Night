using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    public static GameObject Self;
    public int LevelIndex;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Self = gameObject;
        EventManager.instance.Fire(new EnterLevel(LevelIndex, AcrossSceneInfo.GameLevelTimeCount));
    }

    private void Update()
    {
        AcrossSceneInfo.GameLevelTimeCount += Time.deltaTime;
    }
}
