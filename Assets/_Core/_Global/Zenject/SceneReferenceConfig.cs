using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneReferenceConfig", menuName = "Scene game Settings/SceneReferenceConfig")]
public class SceneReferenceConfig : ScriptableObject
{
    [SerializeField] private LocationPlayerController player;
    [SerializeField] private LocationCameraController camera;

    public LocationPlayerController Player => player;
    public LocationCameraController Camera => camera;
}
