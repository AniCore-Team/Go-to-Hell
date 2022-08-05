using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class StartScene : MonoBehaviour
{
    [SerializeField] private float startTime;
    [SerializeField] private string startLevelName = "Location";

    void Start()
    {
        StartCoroutine(waitToStart());
    }

    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(startTime);
        LoadingManager.OnLoadScene.Invoke(startLevelName);
    }
}
