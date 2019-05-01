using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Data
{
    public int CurrentLevel;
}
public class SaveDataManager : MonoBehaviour
{
    public Data data;

    private const string FolderName = "PlayerData";
    private const string FileName = "PlayerData";
    private const string Extension = ".dat";

    private void OnEnable()
    {
        data.CurrentLevel = 0;
        CreateInitData();
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
        data.CurrentLevel = E.Level;
        Data temp = LoadData();
        if (temp.CurrentLevel < data.CurrentLevel)
        {
            SaveData();
        }
    }

    private void CreateInitData()
    {
        string FolderPath = Path.Combine(Application.dataPath, FolderName);
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
        string DataPath = Path.Combine(FolderPath, FileName + Extension);
        if (File.Exists(DataPath))
        {
            return;
        }
        FileStream fileStream = File.Open(DataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    private void SaveData()
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
