using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadingManager : MonoBehaviour
{
    [Inject] private LoadingConfig config;

    public static Action<string> OnLoadScene;
    public event Action onFinishLoad;

    [SerializeField] private GameObject loadingBoard;
    [SerializeField] private Fade fade;
    [SerializeField] private float loadTime;

    private void Awake()
    {
        OnLoadScene += LoadScene;
    }

    private void Start()
    {
        fade.Init(config.FadeSpeed, config.FadeColor);
    }

    public void LoadScene(string name)
    {
        StartCoroutine(waitToLoad(name));
    }

    IEnumerator waitToLoad(string name)
    {
        bool nextStep = false;

        fade.FadeIn(() =>
        {
            loadingBoard.SetActive(true);
            fade.FadeOut(() =>
            {
                nextStep = true;
            });
        });
        yield return new WaitUntil(() => nextStep);
        nextStep = false;

        //var currentSceneName = SceneManager.GetActiveScene().name;
        //AsyncOperation operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        //while (!operation.isDone)
        //{
        //    yield return null;
        //}

        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        //SceneManager.UnloadSceneAsync(currentSceneName);

        SceneManager.LoadScene(name);

        fade.FadeIn(() =>
        {
            loadingBoard.SetActive(false);
            fade.FadeOut(() =>
            {
                nextStep = true;
            });
        });
        yield return new WaitUntil(() => nextStep);
        nextStep = false;
        onFinishLoad?.Invoke();
    }
}
