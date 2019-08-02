using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingLevelData : MonoBehaviour
{
    public string Scene;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        EventManager.instance.AddHandler<EnterLevel>(OnEnterLevel);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterLevel>(OnEnterLevel);
    }

    private void OnEnterLevel(EnterLevel E)
    {
        Destroy(gameObject);
    }
}
