using UnityEngine;
using Zenject;

public class BattleSceneManager : MonoBehaviour
{
    private LevelsConfig levelsConfig;

    private BattleScene currentScene;
    [Inject] private Factory<BattleScene> factory;

    public BattleScene BattleScene => currentScene;

    [Inject]
    public void Construct(LevelsConfig levelsConfig)
    {
        this.levelsConfig = levelsConfig;
    }

    void Start()
    {
        int level = 0;
        int subLevel = Random.Range(0, levelsConfig.LevelsStruct[level].subs.Length);
        SpawnScene(level, subLevel, false);
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
