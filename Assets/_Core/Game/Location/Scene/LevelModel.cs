using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelModel
{
    public Vector3 lastPlayerPosition;
    public Vector3 lastPlayerRotation;
    public List<bool> locationEventObjects = new List<bool>();

    public void Save(string name)
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        FileManager.WriteToFile($"{name}.sav", json);
    }

    public bool Load(string name)
    {
        if (FileManager.LoadFromFile($"{name}.sav", out var json))
        {
            LevelModel loadData = JsonConvert.DeserializeObject<LevelModel>(json);
            lastPlayerPosition = loadData.lastPlayerPosition;
            lastPlayerRotation = loadData.lastPlayerRotation;
            locationEventObjects = loadData.locationEventObjects;
            return true;
        }
        return false;
    }
}
