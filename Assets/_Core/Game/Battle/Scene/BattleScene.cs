using UnityEngine;
using Zenject;

public class BattleScene : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerPosition, enemyPosition;

    private GameObject player;
    private GameObject currentEnemy;

    [Inject] private EnemyList enemyList;
    [Inject] private GameManager gameManager;

    public EnemyController EnemyController => currentEnemy.GetComponent<EnemyController>();
    public BaseCharacter PlayerController => player.GetComponent<BaseCharacter>();

    void Start()
    {
        player = Instantiate(playerPrefab);
        player.transform.position = playerPosition.position;
        player.transform.rotation = playerPosition.rotation;

        currentEnemy = Instantiate(enemyList.list[gameManager.CurrentEnemyID].prefab);
        currentEnemy.transform.position = enemyPosition.position;
        currentEnemy.transform.rotation = enemyPosition.rotation;
    }
}
