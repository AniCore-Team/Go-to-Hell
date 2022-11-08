using ModestTree;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class LevelController : MonoBehaviour
{
    [Inject] private SceneReferenceConfig config;
    [Inject] private GameManager gameManager;
    [SerializeField] private Transform startPlayerPosition;
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] private LocationEventObject[] locationEventObjects;

    private LevelModel levelModel;
    private LocationPlayerController player;

    void Start()
    {
        navMesh.BuildNavMesh();
        levelModel = new();
        InitLocationEventObjects();
        if (!levelModel.Load(gameObject.name))
        {
            ResetTransformToDefault();
        }
        else
        {
            DisableUsedLocationEvent();
        }
        SpawnPlayer();
        SpawnCamera();
    }

    private void Update()
    {
        levelModel.lastPlayerPosition = player.transform.position;
        levelModel.lastPlayerRotation = player.transform.eulerAngles;
    }

    public void UsingLocationEvent(LocationEventObject eventObject)
    {
        var eventIndex = locationEventObjects.IndexOf(eventObject);
        gameManager.SetUsingEvent(eventIndex);
    }

    private void ResetTransformToDefault()
    {
        levelModel.lastPlayerPosition = startPlayerPosition.position;
        levelModel.lastPlayerRotation = startPlayerPosition.rotation.eulerAngles;
    }

    private void InitLocationEventObjects()
    {
        foreach (var locationEvent in locationEventObjects)
        {
            levelModel.locationEventObjects.Add(locationEvent.gameObject.activeSelf);
            locationEvent.SetOwner(this);
        }
    }

    private void DisableUsedLocationEvent()
    {
        var indexEvent = gameManager.GetUsedIndexEvent();
        if (indexEvent != -1)
            levelModel.locationEventObjects[indexEvent] = false;
        for (int i = 0; i < locationEventObjects.Length; i++)
        {
            locationEventObjects[i].gameObject.SetActive(levelModel.locationEventObjects[i]);
        }
    }

    private void SpawnPlayer()
    {
        player = Instantiate(config.Player);
        player.transform.localScale = Vector3.one;
        player.transform.position = levelModel.lastPlayerPosition;
        player.transform.rotation = Quaternion.Euler(levelModel.lastPlayerRotation);
    }

    private void SpawnCamera()
    {
        var camera = Instantiate(config.Camera);
        camera.Init(player.transform);
    }

    private void OnDestroy()
    {
        levelModel.Save(gameObject.name);
    }
}
