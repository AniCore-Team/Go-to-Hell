using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MainScriptableObjectInstaller", menuName = "Installers/MainScriptableObjectInstaller")]
public class MainScriptableObjectInstaller : ScriptableObjectInstaller<MainScriptableObjectInstaller>
{
    [SerializeField] private SceneReferenceConfig sceneReference;
    [SerializeField] private LoadingConfig loadingConfig;
    [SerializeField] private LevelsConfig levelsConfig;
    [SerializeField] private EnemyList enemyList;
    [SerializeField] private CardsList cardsList;
    [SerializeField] private DefaultPlayerCards playerCards;

    public override void InstallBindings()
    {
        Container.BindInstances(
            sceneReference,
            loadingConfig,
            levelsConfig,
            enemyList,
            cardsList,
            playerCards
            );
    }
}