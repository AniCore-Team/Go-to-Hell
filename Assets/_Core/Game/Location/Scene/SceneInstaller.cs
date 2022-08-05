using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<SceneSpawner>().FromNew().AsSingle();
    }
}
