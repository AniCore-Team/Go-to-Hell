using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadingConfig", menuName = "Scene game Settings/LoadingConfig")]
public class LoadingConfig : ScriptableObject
{
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Color fadeColor;

    public LoadingManager LoadingManager => loadingManager;
    public float FadeSpeed => fadeSpeed;
    public Color FadeColor => fadeColor;
}
