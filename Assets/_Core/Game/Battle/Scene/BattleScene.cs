using UnityEngine;
using Zenject;

public class BattleScene : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerPosition, enemyPosition;

    [SerializeField] private BaseCharacter player, enemy;

    //private GameObject player;
    //private GameObject currentEnemy;

    [Inject] private EnemyList enemyList;
    [Inject] private GameManager gameManager;
    [Inject] private CinemachineSwitcher cinemachineSwitcher;

    public EnemyController EnemyController => enemy as EnemyController;
    public PlayerController PlayerController => player as PlayerController;
    public BossProperty BossProperty => enemyList.list[gameManager.CurrentEnemyID] as BossProperty;

    void Start()
    {
        Instantiate(playerPrefab, player.transform);
        player.Init(cinemachineSwitcher);

        var enemyProperty = enemyList.list[gameManager.CurrentEnemyID];
        Instantiate(enemyProperty.prefab, enemy.transform);
        enemy.Init(cinemachineSwitcher);

        gameManager.IsBossBattle = enemyProperty is BossProperty;
    }
}
