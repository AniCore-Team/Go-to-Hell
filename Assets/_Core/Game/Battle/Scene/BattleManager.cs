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
        public BaseCharacter player;
        public EnemyController enemy;
        public CardDetector cardDetector;
        public CardView currentCard;
        public BattleWindow battleWindow;
        public BattleManager battleManager;
        public CinemachineSwitcher cinemachineSwitcher;
        public Action<StateRound> OnNextRound;

        public PlayerStateData(
            BaseCharacter player,
            EnemyController enemy,
            CardDetector cardDetector,
            CardView currentCard,
            BattleWindow battleWindow,
            BattleManager battleManager,
            CinemachineSwitcher cinemachineSwitcher,
            Action<StateRound> OnNextRound
            )
        {
            this.player = player;
            this.enemy = enemy;
            this.cardDetector = cardDetector;
            this.currentCard = currentCard;
            this.battleWindow = battleWindow;
            this.battleManager = battleManager;
            this.cinemachineSwitcher = cinemachineSwitcher;
            this.OnNextRound = OnNextRound;
        }
    }

    public struct PreparePlayerStateData
    {
        public BaseCharacter player;
        public ClientDeck deck;
        public BattleWindow battleWindow;
        public CinemachineSwitcher cinemachineSwitcher;
        public Factory<CardView> factory;

        public PreparePlayerStateData(
            BaseCharacter player,
            ClientDeck deck,
            BattleWindow battleWindow,
            CinemachineSwitcher cinemachineSwitcher,
            Factory<CardView> factory
            )
        {
            this.deck = deck;
            this.battleWindow = battleWindow;
            this.factory = factory;
            this.cinemachineSwitcher = cinemachineSwitcher;
            this.player = player;
        }
    }

    public struct PrepareEnemyStateData
    {
        public BattleWindow battleWindow;
        public EnemyController enemy;
        public CinemachineSwitcher cinemachineSwitcher;

        public PrepareEnemyStateData(
            BattleWindow battleWindow,
            EnemyController enemy,
            CinemachineSwitcher cinemachineSwitcher
            )
        {
            this.battleWindow = battleWindow;
            this.enemy = enemy;
            this.cinemachineSwitcher = cinemachineSwitcher;
        }
    }

    public struct EnemyStateData
    {
        public BaseCharacter player;
        public EnemyController enemy;
        public CinemachineSwitcher cinemachineSwitcher;
        public Action<StateRound> OnNextRound;

        public EnemyStateData(
            BaseCharacter player,
            EnemyController enemy,
            CinemachineSwitcher cinemachineSwitcher,
            Action<StateRound> OnNextRound
            )
        {
            this.player = player;
            this.enemy = enemy;
            this.cinemachineSwitcher = cinemachineSwitcher;
            this.OnNextRound = OnNextRound;
        }
    }

    [SerializeField] private BattleWindow battleWindow;

    private CinemachineSwitcher cinemachineSwitcher;
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

    private CustomSignal onFinishBattle;

    public StateRound StateRound
    {
        get => stateRound;
        set
        {
            Debug.Log($"<color=green>New state:</color> {value}");
            stateRound = value;
        }
    }

    public PlayerStateData GetPlayerStateData() => new PlayerStateData(
        battleSceneManager.BattleScene.PlayerController,
        battleSceneManager.BattleScene.EnemyController,
        cardDetector,
        currentCard,
        battleWindow,
        this,
        cinemachineSwitcher,
        NextRound
        );

    public PreparePlayerStateData GetPreparePlayerStateData() => new PreparePlayerStateData(
        battleSceneManager.BattleScene.PlayerController,
        gameManager.ClientDeck,
        battleWindow,
        cinemachineSwitcher,
        factory);

    public PrepareEnemyStateData GetPrepareEnemyStateData() => new PrepareEnemyStateData(
        battleWindow,
        battleSceneManager.BattleScene.EnemyController,
        cinemachineSwitcher
        );

    public EnemyStateData GetEnemyStateData() => new EnemyStateData(
        battleSceneManager.BattleScene.PlayerController,
        battleSceneManager.BattleScene.EnemyController,
        cinemachineSwitcher,
        NextRound
        );

    [Inject]
    private void Construct(
        CinemachineSwitcher cinemachineSwitcher,
        BattleSceneManager battleSceneManager,
        GameManager gameManager,
        LoadingManager loadingManager,
        Factory<CardView> factory
        )
    {
        this.cinemachineSwitcher = cinemachineSwitcher;
        this.battleSceneManager = battleSceneManager;
        this.gameManager = gameManager;
        this.loadingManager = loadingManager;
        this.factory = factory;
        cardDetector = new CardDetector();
        battleWindow.Init(CheckLock);
        loadingManager.onFinishLoad += StartBattle;
        battleWindow.onPassRound += NextRound;
        StateRound = StateRound.PrePlayer;

        onFinishBattle = OnFinishBattle;
        Translator.Add<InnerProtocol>(onFinishBattle);
    }

    private bool CheckLock(bool isLock)
    {
        if (!isLock)
        {
            battlePoint++;
            battleWindow.RepaintPointText(battlePoint);
            return true;
        }
        else if (battlePoint > 0)
        {
            battlePoint--;
            battleWindow.RepaintPointText(battlePoint);
            return true;
        }
        return false;
    }

    private void StartBattle()
    {
        battleWindow.SetActiveBottomPanel(false);
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

                battleSceneManager.BattleScene.PlayerController.characterHUD = battleWindow.PlayerHUD;
                battleSceneManager.BattleScene.EnemyController.characterHUD = battleWindow.EnemyHUD;
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
        if (StateRound != StateRound.Player) return;
        StateRound = StateRound.PreEnemy;
        battleWindow.SetActiveBottomPanel(false);
    }

    public void NextRound(StateRound nextRound)
    {
        StateRound = nextRound;
    }

    private void OnFinishBattle(Enum code)
    {
        switch (code)
        {
            case InnerProtocol.WinBattle:
                Translator.Remove<InnerProtocol>(onFinishBattle);
                gameManager.SetUsedEvent();
                loadingManager.LoadScene("Location");
                break;
            case InnerProtocol.LoseBattle:
                Translator.Remove<InnerProtocol>(onFinishBattle);
                loadingManager.LoadScene("Location");
                break;
        }
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
