using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;

[System.Serializable]
    public class SaveData
    {
        public string hair;
        public string armor;
        public string helmet;
        public string mask;

        public int coin;
    }

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public CharacterBuilder builder;

    public SaveData saveData = new SaveData();

    int coin;
    public string path;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance.gameObject);
        path = Application.persistentDataPath + "/Data.json"; //경로 수정 필요?
        LoadData();
        
        //DontDestroyOnLoad(this.gameObject);


    }

    void Update()
    {
        //SaveData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(saveData, true);

        saveData.hair = builder.Hair;
        saveData.armor = builder.Armor;
        saveData.helmet = builder.Helmet;
        saveData.mask = builder.Mask;

        saveData.coin = coin;

        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        string json = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(json);
    }
}