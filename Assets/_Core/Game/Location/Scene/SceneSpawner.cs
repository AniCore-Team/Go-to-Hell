using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneSpawner
{
    [Inject] private LevelsConfig config;
    [Inject] private Factory<LevelController> levelFactory;

    public void SpawnLevel(int ID, int sub = -1)
    {
        LevelController obj = null;

        obj = config.LevelsStruct[ID].level;

        var level = levelFactory.Create(obj.gameObject);
        level.transform.position = Vector3.zero;
        level.transform.localScale = Vector3.one;
    }
}
