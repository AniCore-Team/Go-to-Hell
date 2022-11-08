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

    //public EnemyController EnemyController => currentEnemy.GetComponent<EnemyController>();
    //public PlayerController PlayerController => player.GetComponent<PlayerController>();
    public EnemyController EnemyController => enemy as EnemyController;
    public PlayerController PlayerController => player as PlayerController;

    void Start()
    {
        /*player = */Instantiate(playerPrefab, player.transform);
        player.Init(cinemachineSwitcher);
        //player.transform.position = playerPosition.position;
        //player.transform.rotation = playerPosition.rotation;

        /*currentEnemy = */Instantiate(enemyList.list[gameManager.CurrentEnemyID].prefab, enemy.transform);
        enemy.Init(cinemachineSwitcher);
        //currentEnemy.transform.position = enemyPosition.position;
        //currentEnemy.transform.rotation = enemyPosition.rotation;
    }
}
