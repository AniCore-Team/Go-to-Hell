using UnityEngine;

public class LocationEvents
{
    public static void SendEvent(LocationEventType type)
    {
        switch (type)
        {
            case LocationEventType.BATTLE:
                Debug.Log("Battle event Start");
                LoadingManager.OnLoadScene.Invoke("Battle");
                break;
            case LocationEventType.SEX:
                Debug.Log("Sex event Start");
                LoadingManager.OnLoadScene.Invoke("Sex");
                break;
            case LocationEventType.MEETING:
                Debug.Log("Meeting event Start");
                break;
        }
    }
}

public enum LocationEventType
{
    BATTLE,
    SEX,
    MEETING
}