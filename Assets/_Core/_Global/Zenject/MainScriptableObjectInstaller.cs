using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MainScriptableObjectInstaller", menuName = "Installers/MainScriptableObjectInstaller")]
public class MainScriptableObjectInstaller : ScriptableObjectInstaller<MainScriptableObjectInstaller>
{
    [SerializeField] private SceneReferenceConfig sceneReference;
    [SerializeField] private LoadingConfig loadingConfig;
    [SerializeField] private LevelsConfig levelsConfig;

    public override void InstallBindings()
    {
        Container.BindInstances(
            sceneReference,
            loadingConfig,
            levelsConfig
            );
    }
}