using UI.Menu;
using UnityEngine;
using Zenject;

public class MenuFactoriesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<GameObject, GameSlot, Factory<GameSlot>>().FromFactory<ZenFactory<GameSlot>>();
    }
}


