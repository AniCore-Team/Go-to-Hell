using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneController : MonoBehaviour
{
    [Inject] private SceneSpawner spawner;

    private void Awake()
    {
        spawner.SpawnLevel(0);
    }

}
