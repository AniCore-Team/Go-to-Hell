using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform startPlayerPosition;
    [SerializeField] private GameObject playerPrefab;

    private Vector3 lastPlayerPosition;
    private Vector3 lastPlayerRotation;

    void Start()
    {
        ResetTransformToDefault();
        SpawnPlayer();
    }

    void Update()
    {
        
    }

    private void ResetTransformToDefault()
    {
        lastPlayerPosition = startPlayerPosition.position;
        lastPlayerRotation = startPlayerPosition.rotation.eulerAngles;
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab);
        player.transform.localScale = Vector3.one;
        player.transform.position = lastPlayerPosition;
        player.transform.rotation = Quaternion.Euler(lastPlayerRotation);
    }
}
