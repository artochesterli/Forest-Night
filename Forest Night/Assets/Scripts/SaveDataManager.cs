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
    public List<int> Progress;

    public Data()
    {
        CurrentSaveSlot = -1;
        Progress = new List<int>();
        for(int i = 0; i < 3; i++)
        {
            Progress.Add(0);
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
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveHandler<EnterLevel>(OnEnterLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnterLevel(EnterLevel E)
    {
        data.Progress[data.CurrentSaveSlot] = E.Level;
        SaveData();
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
