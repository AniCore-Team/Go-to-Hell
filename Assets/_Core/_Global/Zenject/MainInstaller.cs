using Zenject;

public class MainInstaller : MonoInstaller
{
    private LoadingConfig loadingConfig;

    [Inject]
    public void Construct(LoadingConfig loadingConfig)
    {
        this.loadingConfig = loadingConfig;
    }

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromNewComponentOnNewGameObject().WithGameObjectName("GameManager").UnderTransform(transform).AsSingle().NonLazy();
        Container.Bind<Client>().FromNewComponentOnNewGameObject().WithGameObjectName("Client").UnderTransform(transform).AsSingle().NonLazy();
        Container.Bind<LoadingManager>().FromComponentInNewPrefab(loadingConfig.LoadingManager).AsSingle().NonLazy();
    }
}