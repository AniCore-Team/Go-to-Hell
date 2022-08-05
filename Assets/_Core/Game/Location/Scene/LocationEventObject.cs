using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEventObject : MonoBehaviour
{
    [SerializeField] private LocationEventType currentType;
    private bool isEntered;

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
        LocationEvents.SendEvent(currentType);
    }
}
