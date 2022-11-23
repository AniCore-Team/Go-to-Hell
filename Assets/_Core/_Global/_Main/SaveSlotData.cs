using System;
using UnityEngine;

[Serializable]
public class SaveSlotData
{
    public int id;
    [NonSerialized]public Sprite sprite;
    public ClientModel clientModel;
    public LevelModel levelModel;
}