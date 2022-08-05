using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class LevelController : MonoBehaviour
{
    [Inject] private SceneReferenceConfig config;
    [SerializeField] private Transform startPlayerPosition;
    [SerializeField] private NavMeshSurface navMesh;

    private Vector3 lastPlayerPosition;
    private Vector3 lastPlayerRotation;

    private LocationPlayerController player;

    void Start()
    {
        navMesh.BuildNavMesh();
        ResetTransformToDefault();
        SpawnPlayer();
        SpawnCamera();
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
        player = Instantiate(config.Player);
        player.transform.localScale = Vector3.one;
        player.transform.position = lastPlayerPosition;
        player.transform.rotation = Quaternion.Euler(lastPlayerRotation);
    }

    private void SpawnCamera()
    {
        var camera = Instantiate(config.Camera);
        camera.Init(player.transform);
    }
}
