using Common;
using PureAnimator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardAction
{
    public event Action OnFinishedCast;

    public string NameAnimation { get; }

    public void Cast(BaseCharacter self, BaseCharacter[] other);
    public void Tick(BaseCharacter self, BaseCharacter other);
    public void End();
}

public class Effect
{
    private CardID id;
    private int duration;
    private TargetEffect target;
    private int count;

    private ICardAction cardAction;
    public event Action OnFinishedCast;

    public CardID ID => id;
    public bool CheckDuration => count >= duration;

    public Effect(CardID id, int duration, TargetEffect target)
    {
        this.id = id;
        this.duration = duration;
        this.target = target;
        count = 0;
    }

    public Effect(ICardAction action)
    {
        cardAction = action;
    }


    public void PowerUp(CardProperty newCard)
    {

    }

    public void PowerUp(Effect effect)
    {

    }

    public void BeginAction(Func<TargetEffect, BaseCharacter[]> getCharacter)
    {
        getCharacter(TargetEffect.Self)[0].Attack(cardAction.NameAnimation);
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(0.1f, progress =>
            {
                return default;
            }, () =>
            {
                //var allTimeAnimation = getCharacter(TargetEffect.Self)[0].GetLegthAnimation();
                var eventTimeAnimations = getCharacter(TargetEffect.Self)[0].GetEventTimeAnimation();

                for (var i = 0; i < eventTimeAnimations.Length; i++)
                {
                    Services<PureAnimatorController>
                        .Get()
                        .GetPureAnimator()
                        .Play(eventTimeAnimations[i], progress =>
                        {
                            return default;
                        }, () =>
                        {
                            cardAction.OnFinishedCast += OnFinishedCast;
                            cardAction.Cast(getCharacter(TargetEffect.Self)[0], getCharacter(TargetEffect.Other));
                        });
                }
            });
    }

    public void RoundAction()
    {
        count++;
    }

    public void EndAction()
    {
        cardAction.OnFinishedCast -= OnFinishedCast;
        OnFinishedCast = null;
    }
}

public class CardEffectsController
{
    private Dictionary<CardID, Effect> effects = new Dictionary<CardID, Effect>();
    private List<CardID> dropEffects = new List<CardID>();

    private BaseCharacter selfCharacter;
    private BaseCharacter otherCharacter;

    public void Init(BaseCharacter self, BaseCharacter other)
    {
        selfCharacter = self;
        otherCharacter = other;
    }
    
    public void AddEffect(CardProperty newCard, Action finishedCast)
    {
        //var effect = new Effect
        //(
        //    newCard.id,
        //    newCard.duration,
        //    newCard.target
        //);
        var effect = new Effect(newCard.effectAction);
        effect.OnFinishedCast += finishedCast;
        effect.BeginAction(GetCharacters);

        if (newCard.duration > 0)
        {
            if (effects.ContainsKey(newCard.id))
                effects[newCard.id].PowerUp(effect);
            else
                effects.Add(newCard.id, effect);
        }
    }

    public void Tick()
    {
        foreach (Effect effect in effects.Values)
        {
            effect.RoundAction();
            if (effect.CheckDuration)
            {
                effect.EndAction();
                dropEffects.Add(effect.ID);
            }
        }
        DropEffects();
    }

    private void DropEffects()
    {
        foreach(CardID card in dropEffects)
            effects.Remove(card);

        dropEffects.Clear();
    }

    private BaseCharacter[] GetCharacters(TargetEffect target)
    {
        List<BaseCharacter> characters = new List<BaseCharacter>();

        switch (target)
        {
            case TargetEffect.All:
                characters.Add(selfCharacter);
                characters.Add(otherCharacter);
                break;
            case TargetEffect.Self:
                characters.Add(selfCharacter);
                break;
            case TargetEffect.Other:
                characters.Add(otherCharacter);
                break;
        }

        return characters.ToArray();
    }
}
