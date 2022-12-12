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
    public SubLevel bossScene;
}

[System.Serializable]
public struct SubLevel
{
    public string name;
    public BattleScene level;
    public bool isBoss;
}