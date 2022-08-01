using Zenject;

public class MainInstaller : MonoInstaller
{
    [Inject] private SceneReferenceConfig sceneReference;

    public override void InstallBindings()
    {
       /* Container.Bind<GameManager>().FromNewComponentOnNewGameObject().WithGameObjectName("GameManager").UnderTransform(transform).AsSingle().NonLazy();
        Container.Bind<UIController>().FromComponentInNewPrefab(sceneReference.uiController).AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInNewPrefab(sceneReference.player).AsSingle().NonLazy();
        Container.Bind<CameraController>().FromComponentInNewPrefab(sceneReference.camera).AsSingle().NonLazy();
        Container.Bind<ChestInventory>().FromComponentInNewPrefab(sceneReference.chestInventory).AsSingle().NonLazy();
        Container.Bind<ChestUIInventory>().FromComponentInNewPrefab(sceneReference.chestUI).AsSingle().NonLazy();
        Container.Bind<PlayerInventory>().FromNew().AsSingle().NonLazy();
        Container.Bind<PlayerMovement>().FromNew().AsSingle().NonLazy();
        Container.Bind<PlayerEquipment>().FromNew().AsSingle().NonLazy();
        Container.Bind<PlayerSaveInventory>().FromNew().AsSingle().NonLazy();
        Container.Bind<PlayerBattle>().FromNew().AsSingle().NonLazy();
        Container.Bind<PlayerSave>().FromNew().AsSingle().NonLazy();
        Container.Bind<SaveChestInventory>().FromNew().AsSingle().NonLazy();*/
    }
}