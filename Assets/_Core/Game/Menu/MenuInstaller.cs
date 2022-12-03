using UI.Menu;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WindowsManager>().FromNew().AsSingle().NonLazy();
        Container.Bind<MenuManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CreateNewGameWindow>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameSlotsWindow>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<MenuButtonsWindow>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<SettingsWindow>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
