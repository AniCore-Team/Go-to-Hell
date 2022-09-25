using UnityEngine;
using Zenject;


public class FactoriesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<GameObject, LevelController, Factory<LevelController>>().FromFactory<ZenFactory<LevelController>>();
        Container.BindFactory<GameObject, BattleScene, Factory<BattleScene>>().FromFactory<ZenFactory<BattleScene>>();
        Container.BindFactory<GameObject, CardView, Factory<CardView>>().FromFactory<ZenFactory<CardView>>();

    }
}


