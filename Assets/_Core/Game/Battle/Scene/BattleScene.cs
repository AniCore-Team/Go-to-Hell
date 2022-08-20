using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        player = Instantiate(playerPrefab);
        player.transform.position = playerPosition.position;
        player.transform.rotation = playerPosition.rotation;

        currentEnemy = Instantiate(enemyList.list[gameManager.CurrentEnemyID].prefab);
        currentEnemy.transform.position = enemyPosition.position;
        currentEnemy.transform.rotation = enemyPosition.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
