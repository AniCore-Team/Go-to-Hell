using Zenject;

public class BattleInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BattleSceneManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CinemachineSwitcher>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
