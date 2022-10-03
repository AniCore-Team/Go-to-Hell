using Common;
using PureAnimator;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class BattleManager : MonoBehaviour
{
    public struct PlayerStateData
    {
        public CardDetector cardDetector;
        public CardView currentCard;
        public BattleWindow battleWindow;

        public PlayerStateData(CardDetector cardDetector, CardView currentCard, BattleWindow battleWindow)
        {
            this.cardDetector = cardDetector;
            this.currentCard = currentCard;
            this.battleWindow = battleWindow;
        }
    }

    public struct PreparePlayerStateData
    {
        public ClientDeck deck;
        public BattleWindow battleWindow;
        public Factory<CardView> factory;

        public PreparePlayerStateData(ClientDeck deck, BattleWindow battleWindow, Factory<CardView> factory)
        {
            this.deck = deck;
            this.battleWindow = battleWindow;
            this.factory = factory;
        }
    }

    public struct PrepareEnemyStateData
    {
        public BattleWindow battleWindow;

        public PrepareEnemyStateData(BattleWindow battleWindow)
        {
            this.battleWindow = battleWindow;
        }
    }

    public struct EnemyStateData
    {
        public EnemyController enemy;

        public EnemyStateData(EnemyController enemy)
        {
            this.enemy = enemy;
        }
    }

    [SerializeField] private BattleWindow battleWindow;

    private BattleSceneManager battleSceneManager;
    private GameManager gameManager;
    private LoadingManager loadingManager;
    private Factory<CardView> factory;

    [SerializeField] private CardDetector cardDetector;
    [SerializeField] private BattleBehaviourGraph graph;
    private BattleBehaviourController battleController;
    private CardView currentCard;
    public StateRound stateRound = StateRound.None;

    [SerializeField] public int battlePoint = 0;

    //public StateRound StateRound => stateRound;

    public PlayerStateData GetPlayerStateData() => new PlayerStateData(
        cardDetector,
        currentCard,
        battleWindow);

    public PreparePlayerStateData GetPreparePlayerStateData() => new PreparePlayerStateData(
        gameManager.ClientDeck,
        battleWindow,
        factory);

    public PrepareEnemyStateData GetPrepareEnemyStateData() => new PrepareEnemyStateData(
        battleWindow);

    public EnemyStateData GetEnemyStateData() => new EnemyStateData(
        battleSceneManager.BattleScene.EnemyController);

    [Inject]
    private void Construct(
        BattleSceneManager battleSceneManager,
        GameManager gameManager,
        LoadingManager loadingManager,
        Factory<CardView> factory
        )
    {
        this.battleSceneManager = battleSceneManager;
        this.gameManager = gameManager;
        this.loadingManager = loadingManager;
        this.factory = factory;
        cardDetector = new CardDetector();
        battleWindow.Init(CheckLock);
        loadingManager.onFinishLoad += StartBattle;
        battleWindow.onPassRound += NextRound;
        stateRound = StateRound.PrePlayer;
    }

    private bool CheckLock(bool isLock)
    {
        if (!isLock)
        {
            battlePoint++;
            battleWindow.SetPointText(battlePoint);
            return true;
        }
        else if (battlePoint > 0)
        {
            battlePoint--;
            battleWindow.SetPointText(battlePoint);
            return true;
        }
        return false;
    }

    private void StartBattle()
    {
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(0.2f, progress =>
            {
                return default;
            }, () =>
            {
                battleController = new BattleBehaviourController();
                battleController.TryInstall(this, graph);
            });
    }

    private void Update()
    {
        battleController?.BehaviourUpdate();
    }

    private void LateUpdate()
    {
        battleController?.LateBehaviourUpdate();
    }

    private void OnDestroy()
    {
        loadingManager.onFinishLoad -= StartBattle;
    }

    public void NextRound()
    {
        stateRound = StateRound.PreEnemy;
    }
}

[Serializable]
public enum StateRound
{
    None,
    PrePlayer,
    Player,
    PreEnemy,
    Enemy
}

[Serializable]
public class CardDetector
{
    private Camera mainCamera;
    [SerializeField] private CardView targetObject;
    private CardView prevTargetObject;

    public CardView TargetObject => targetObject;
    public bool isTarget { get; private set; } = false;
    private Camera Camera
    {
        get
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            return mainCamera;
        }
    }
    public void Update()
    {
        var ray = Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            targetObject = hit.collider.GetComponent<CardView>();
            isTarget = true;
        }
        else
        {
            isTarget = false;
        }

        if (isTarget && targetObject != null)
        {
            targetObject.HightLight(true);

            if (prevTargetObject == null)
            {
                prevTargetObject = targetObject;
            }
            else if (prevTargetObject != targetObject)
            {
                prevTargetObject.HightLight(false);
                prevTargetObject = targetObject;
            }
        }
        else
        {
            isTarget = false;
            prevTargetObject?.HightLight(false);
            prevTargetObject = null;
        }
    }
}
