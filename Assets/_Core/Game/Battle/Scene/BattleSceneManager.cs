using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleSceneManager : MonoBehaviour
{
    private LevelsConfig levelsConfig;

    private BattleScene currentScene;
    [Inject] private Factory<BattleScene> factory;

    [Inject]
    public void Construct(LevelsConfig levelsConfig)
    {
        this.levelsConfig = levelsConfig;
    }

    void Start()
    {
        SpawnScene(0, 0, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnScene(int level, int sub, bool isBoss)
    {
        var scene = levelsConfig.LevelsStruct[level].subs[sub];

        var lvl = factory.Create(scene.level.gameObject);
        lvl.transform.position = Vector3.zero;
        lvl.transform.localScale = Vector3.one;
        currentScene = lvl;
    }
}
