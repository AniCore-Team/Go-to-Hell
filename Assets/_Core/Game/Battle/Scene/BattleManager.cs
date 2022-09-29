using Common;
using PureAnimator;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleWindow battleWindow;

    private GameManager gameManager;
    private LoadingManager loadingManager;
    private Factory<CardView> factory;

    [SerializeField] private CardDetector cardDetector;
    private CardView currentCard;
    private Ray rayToCursor;

    private int battlePoint = 0;

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
        battleWindow.Init(IsLock);
        loadingManager.onFinishLoad += StartBattle;
    }

    private bool IsLock(bool isLock)
    {
        if (!isLock)
        {
            battlePoint++;
            return true;
        }
        else if (battlePoint > 0)
        {
            battlePoint--;
            return true;
        }
        return false;
    }

    private void StartBattle()
    {
        //yield return null;
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(2f, progress =>
            {
                return default;
            }, () =>
            {
                StartRound();
                battleWindow.onPassRound += NextRound;
            });
    }

    private void Update()
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
