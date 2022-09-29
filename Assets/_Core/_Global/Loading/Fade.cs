using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Common;
using PureAnimator;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image fadeIMG;
    private float deltaTime;

    public Action<bool> endFade;

    public void Init(float time, Color color)
    {
        deltaTime = time;
        fadeIMG.color = color;
    }

    public void FadeIn(Action endCommand)
    {
        Color color = fadeIMG.color;
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(deltaTime, progress =>
            {
                color.a = progress;
                fadeIMG.color = color;
                return default;
            }, () =>
            {
                endCommand?.Invoke();
            });
    }

    public void FadeOut(Action endCommand)
    {
        Color color = fadeIMG.color;
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(deltaTime, progress =>
            {
                color.a = 1 - progress;
                fadeIMG.color = color;
                return default;
            }, () =>
            {
                endCommand?.Invoke();
            });
    }
}
