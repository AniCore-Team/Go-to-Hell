using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Scene game Settings/LevelsConfig")]
public class LevelsConfig : ScriptableObject
{
    [SerializeField] private Level[] levelsStruct;

    public Level[] LevelsStruct => levelsStruct;
}

[System.Serializable]
public struct Level
{
    public string levelName;
    public LevelController level;
    public SubLevel[] subs;
}

[System.Serializable]
public struct SubLevel
{
    public GameObject level;
}