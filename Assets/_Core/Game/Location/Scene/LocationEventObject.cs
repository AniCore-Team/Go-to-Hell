using Sources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LocationEventObject : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private LocationEventType currentType;
    [SerializeField] private int enemyID;
    private LevelController levelController;
    private bool isEntered;

    [Inject] private GameManager gameManager;
    [Inject] private AudioManager audioManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LocationPlayerController>(out var player))
        {
            isEntered = true;
            audioManager.PlaySound3D("TriggerEnter", transform.position);
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

    public void SetOwner(LevelController levelController)
    {
        this.levelController = levelController;
    }

    private void SendEvent()
    {
        if(currentType == LocationEventType.BATTLE)
        {
            gameManager.SetCurrentEnemy(enemyID);
        }

        levelController.UsingLocationEvent(this);
        levelController.StartDialogue(dialogueData);
        //LocationEvents.SendEvent(currentType);
    }
}
