using Zenject;

public class MenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MenuManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CreateNewGameWindow>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
