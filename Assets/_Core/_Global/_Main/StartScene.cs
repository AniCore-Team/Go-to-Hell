using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class StartScene : MonoBehaviour
{
    [SerializeField] private float startTime;

    [Inject] private LoadingManager loadingManager;

    void Start()
    {
        StartCoroutine(waitToStart());
    }

    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(startTime);
        loadingManager.LoadScene("Menu");
    }
}
