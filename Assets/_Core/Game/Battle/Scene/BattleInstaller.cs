using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BattleSceneManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
