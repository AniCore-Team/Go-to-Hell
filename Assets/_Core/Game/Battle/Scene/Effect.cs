﻿using Common;
using PureAnimator;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    private CardID id;
    public int duration;
    private TargetEffect target;
    public TypeEffect typeEffect;
    private int count;
    public bool isPowered;
    private List<GameObject> longTimeObjects = new List<GameObject>();

    private ICardAction cardAction;
    private Func<TargetEffect, BaseCharacter[]> getCharacter;

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
        cardAction.PowerUp(this);
    }

    public void BeginAction(Func<TargetEffect, BaseCharacter[]> getCharacter, Action finishedCast)
    {
        this.getCharacter = getCharacter;
        AsyncWaitAnimationEvent(finishedCast);
    }

    public void RoundAction(Action endRoundAction)
    {
        count++;
        cardAction.Tick(
            this,
            getCharacter(target)[0],
            getCharacter(target == TargetEffect.Self ? TargetEffect.Other : TargetEffect.Self),
            endRoundAction);
    }

    public void EndAction(Action endTick)
    {
        cardAction.End(endTick,
            getCharacter(target)[0],
            getCharacter(target == TargetEffect.Self ? TargetEffect.Other : TargetEffect.Self),
            this);
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

    private void AsyncWaitAnimationEvent(Action finishedCast)
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
                    CastToEvent(finishedCast);
                });
            }
        });
    }

    private void CastToEvent(Action finishedCast)
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
