using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Data
{
    public int CurrentSaveSlot;
    public List<bool> Finish;
    public List<int> Progress;
    public List<float> Time;

    private const int SlotNum=3;

    public Data()
    {
        CurrentSaveSlot = -1;
        Progress = new List<int>();
        Time = new List<float>();
        Finish = new List<bool>();
        for(int i = 0; i < SlotNum; i++)
        {
            Progress.Add(0);
            Time.Add(0);
            Finish.Add(false);
        }
    }
}


public class SaveDataManager : MonoBehaviour
{
    public static Data data;

    private const string FolderName = "PlayerData";
    private const string FileName = "PlayerData";
    private const string Extension = ".dat";

    private void OnEnable()
    {
        if (data == null)
        {
            CreateInitData();
        }
        EventManager.instance.AddHandler<EnterLevel>(OnEnterLevel);
        EventManager.instance.AddHandler<QuitGame>(OnQuitGame);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterLevel>(OnEnterLevel);
        EventManager.instance.RemoveHandler<QuitGame>(OnQuitGame);
    }

    private void OnEnterLevel(EnterLevel E)
    {
        data.Progress[data.CurrentSaveSlot] = E.Level;
        data.Time[data.CurrentSaveSlot] += AcrossSceneInfo.GameLevelTimeCount;
        SaveData();
        AcrossSceneInfo.GameLevelTimeCount = 0;
    }

    private void OnQuitGame(QuitGame Q)
    {
        if (Q.finish)
        {
            data.Finish[data.CurrentSaveSlot] = true;
        }
        data.Time[data.CurrentSaveSlot] += AcrossSceneInfo.GameLevelTimeCount;
        SaveData();
        AcrossSceneInfo.GameLevelTimeCount = 0;
        
    }

    public static void CreateInitData()
    {
        string FolderPath = Path.Combine(Application.dataPath, FolderName);
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
        string DataPath = Path.Combine(FolderPath, FileName + Extension);
        if (File.Exists(DataPath))
        {
            data=LoadData();
            return;
        }

        data = new Data();

        FileStream fileStream = File.Open(DataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static void SaveData()
    {
        string FolderPath = Path.Combine(Application.dataPath, FolderName);
        string DataPath = Path.Combine(FolderPath, FileName + Extension);
        FileStream fileStream = File.Open(DataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static Data LoadData()
    {
        string FolderPath = Path.Combine(Application.dataPath, FolderName);
        string DataPath = Path.Combine(FolderPath, FileName + Extension);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(DataPath, FileMode.Open);
        Data Value = (Data)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();
        return Value;

    }
}
