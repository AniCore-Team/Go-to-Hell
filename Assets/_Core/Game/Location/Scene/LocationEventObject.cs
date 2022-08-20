using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LocationEventObject : MonoBehaviour
{
    [SerializeField] private LocationEventType currentType;
    [SerializeField] private int enemyID;
    private bool isEntered;

    [Inject] private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LocationPlayerController>(out var player))
        {
            isEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isEntered = false;
    }

    private void Update()
    {
        if(isEntered)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SendEvent();
                isEntered = false;
            }
        }
    }

    private void SendEvent()
    {
        if(currentType == LocationEventType.BATTLE)
        {
            gameManager.SetCurrentEnemy(enemyID);
        }

        LocationEvents.SendEvent(currentType);
    }
}
