using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneSpawner
{
    [Inject] private LevelsConfig config;

    public void SpawnLevel(int ID, int sub = -1)
    {
        GameObject obj = null;

        if (sub == -1)
            obj = config.LevelsStruct[ID].level;
        else
            obj = config.LevelsStruct[ID].subs[sub].level;

        var level = Object.Instantiate(obj);
        level.transform.position = Vector3.zero;
        level.transform.localScale = Vector3.one;
    }
}
