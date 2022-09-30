using Common;
using PureAnimator;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleWindow battleWindow;

    private GameManager gameManager;
    private LoadingManager loadingManager;
    private Factory<CardView> factory;

    [SerializeField] private CardDetector cardDetector;
    [SerializeField] private BattleBehaviourGraph graph;
    private BattleBehaviourController battleController;
    private CardView currentCard;
    private StateRound stateRound;

    private int battlePoint = 0;

    public StateRound StateRound => stateRound;

    [Inject]
    private void Construct(
        GameManager gameManager,
        LoadingManager loadingManager,
        Factory<CardView> factory
        )
    {
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
                //StartRound();
            });
    }

    private void Update()
    {
        battleController.BehaviourUpdate();
    }

    private void LateUpdate()
    {
        battleController.LateBehaviourUpdate();
    }

    public void UpdateRound()
    {
        cardDetector.Update();
        if (cardDetector.isTarget)
        {
            currentCard = cardDetector.TargetObject;
            if (Input.GetMouseButtonUp(0))
            {
                if (battlePoint < currentCard.property.card.cost) return;

                currentCard.Use(battleWindow.ShiftToFreeSlots);
                battlePoint -= currentCard.property.card.cost;
                battleWindow.SetPointText(battlePoint);
            }
        }
    }

    private void OnDestroy()
    {
        loadingManager.onFinishLoad -= StartBattle;
    }

    private void NextRound()
    {
        stateRound = StateRound.PreEnemy;
        StartCoroutine(ChangeRound());
    }

    public void StartRound()
    {
        battlePoint += 10;
        battleWindow.SetPointText(battlePoint);
        var count = battleWindow.GetCountFreeSlots();

        CardView[] newCards = new CardView[count];
        for (int i = 0; i < count; i++)
        {
            var randomCard = gameManager.ClientDeck.GetRandomCard();
            newCards[i] = factory.Create(randomCard.card.prefab);
            newCards[i].property = randomCard;
        }
        battleWindow.SetCard(newCards);
        stateRound = StateRound.Player;
    }

    private IEnumerator ChangeRound()
    {
        yield return EndRound();

        StartRound();
    }

    private IEnumerator EndRound()
    {
        bool nextStep = false;

        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(1f, progress =>
            {
                battleWindow.HideUnlockedCard(1 - progress);
                return default;
            }, () =>
            {
                nextStep = true;
            });

        yield return new WaitUntil(() => nextStep);
        battleWindow.UnlockedAndClearCards();
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
