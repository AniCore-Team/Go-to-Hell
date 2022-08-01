using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadingManager : MonoBehaviour
{
    [Inject] private LoadingConfig config;

    [SerializeField] private GameObject loadingBoard;
    [SerializeField] private Fade fade;
    [SerializeField] private float loadTime;

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
        fade.FadeIn();
        yield return new WaitForSeconds(config.FadeSpeed/2);
        loadingBoard.SetActive(true);
        fade.FadeOut();
        yield return new WaitForSeconds(config.FadeSpeed / 2);
        SceneManager.LoadScene(name);
        yield return new WaitForSeconds(loadTime);
        fade.FadeIn();
        yield return new WaitForSeconds(config.FadeSpeed / 2);
        loadingBoard.SetActive(false);
        yield return new WaitForSeconds(config.FadeSpeed / 2);

        fade.FadeOut();
    }
}
