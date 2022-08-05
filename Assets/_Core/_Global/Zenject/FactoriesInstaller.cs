using UnityEngine;
using Zenject;


public class FactoriesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<GameObject, LevelController, Factory<LevelController>>().FromFactory<ZenFactory<LevelController>>();


    }
}


