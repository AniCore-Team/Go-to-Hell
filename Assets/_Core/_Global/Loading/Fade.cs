using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image fadeIMG;
    private float deltaTime;

    public Action<bool> endFade;

    private bool inOut;
    private bool IsFade;

    public void Init(float time, Color color)
    {
        deltaTime = time;
        fadeIMG.color = color;
    }

    public void FadeIn()
    {
        inOut = true;
        IsFade = true;
    }

    public void FadeOut()
    {
        inOut = false;
        IsFade = true;
    }

    private void LateUpdate()
    {
        if(IsFade)
        {
            Color color = fadeIMG.color;
            
            if(inOut)
            {
                if (fadeIMG.color.a < 255)
                {
                    color.a += deltaTime * Time.deltaTime;
                    fadeIMG.color = color;
                }
                else
                {
                    IsFade = false;
                }
                
            }
            else
            {
                if (fadeIMG.color.a > 0)
                {
                    color.a -= deltaTime * Time.deltaTime;
                    fadeIMG.color = color;
                }
                else
                {
                    IsFade = false;
                }
            }
        }
        else
        { return; }
    }
}
