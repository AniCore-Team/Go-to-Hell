using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MainScriptableObjectInstaller", menuName = "Installers/MainScriptableObjectInstaller")]
public class MainScriptableObjectInstaller : ScriptableObjectInstaller<MainScriptableObjectInstaller>
{
    [SerializeField] private SceneReferenceConfig sceneReference;

    public override void InstallBindings()
    {
        Container.BindInstances(
            sceneReference
            );

    }
}