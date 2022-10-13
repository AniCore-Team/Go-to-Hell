using Common;
using PureAnimator;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    private CardID id;
    private int duration;
    private TargetEffect target;
    public TypeEffect typeEffect;
    private int count;
    public bool isPowered;
    private List<GameObject> longTimeObjects = new List<GameObject>();

    private ICardAction cardAction;
    //public event Action OnFinishedCast;

    public CardID ID => id;
    public bool CheckDuration => count >= duration;
    public bool HasContainsLongTimeObjects => longTimeObjects.Count > 0;

    private PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public Effect(CardID id, ICardAction action)
    {
        this.id = id;
        target = action.TargetEffect;
        duration = action.Duration;
        cardAction = action;
        typeEffect = action.TypeEffect;
    }

    public void PowerUp(CardProperty newCard)
    {

    }

    public void PowerUp(Effect effect)
    {
        isPowered = true;
    }

    public void BeginAction(Func<TargetEffect, BaseCharacter[]> getCharacter, Action finishedCast)
    {
        AsyncWaitAnimationEvent(getCharacter, finishedCast);
    }

    public void RoundAction(Action endRoundAction)
    {
        count++;
        endRoundAction?.Invoke();
    }

    public void EndAction(Action endTick)
    {
        cardAction.End(endTick, this);
    }

    public void AddLongTimeObjects(params GameObject[] newObjects)
    {
        foreach (var item in newObjects)
        {
            longTimeObjects.Add(item);
        }
    }

    public GameObject[] GetLongTimeObjects()
    {
        return longTimeObjects.ToArray();
    }

    public void ClearLongTimeObjects()
    {
        longTimeObjects.Clear();
    }

    private void AsyncWaitAnimationEvent(Func<TargetEffect, BaseCharacter[]> getCharacter, Action finishedCast)
    {
        getCharacter(target)[0].Attack(cardAction.NameAnimation);
        PureAnimation.Play(0.1f, Utils.EmptyPureAnimation, () =>
        {
            //var allTimeAnimation = getCharacter(TargetEffect.Self)[0].GetLegthAnimation();
            var eventTimeAnimations = getCharacter(target)[0].GetEventTimeAnimation();

            for (var i = 0; i < eventTimeAnimations.Length; i++)
            {
                PureAnimation.Play(eventTimeAnimations[i], Utils.EmptyPureAnimation, () =>
                {
                    CastToEvent(getCharacter, finishedCast);
                });
            }
        });
    }

    private void CastToEvent(Func<TargetEffect, BaseCharacter[]> getCharacter, Action finishedCast)
    {
        //cardAction.OnFinishedCast += OnFinishedCast;
        cardAction.Cast(
            this,
            getCharacter(target)[0],
            getCharacter(target == TargetEffect.Self ? TargetEffect.Other : TargetEffect.Self),
            finishedCast
            );
    }
}
