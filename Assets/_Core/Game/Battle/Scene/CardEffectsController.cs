using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectsController
{
    private Dictionary<CardID, Effect> effects = new Dictionary<CardID, Effect>();
    private Queue<Action<Action>> usingTicks = new Queue<Action<Action>>();
    private List<CardID> dropEffects = new List<CardID>();

    private BaseCharacter selfCharacter;
    private BaseCharacter otherCharacter;
    private Action end;

    public bool IsStun => effects.Values.Any(effect => effect.typeEffect == TypeEffect.Stun);

    public void Init(BaseCharacter self, BaseCharacter other)
    {
        selfCharacter = self;
        otherCharacter = other;
    }

    public bool ContainsLongTimeObjects(CardID cardID)
    {
        if (effects.ContainsKey(cardID))
            return effects[cardID].HasContainsLongTimeObjects;
        else
            return false;
    }

    public Effect GetEffect(CardID cardID)
        => effects[cardID];

    public void AddEffect(CardProperty newCard, Action finishedCast, bool fastCast = false)
    {
        var effect = new Effect(newCard.id, newCard.effectAction);
        bool blocked = newCard.effectAction.TargetEffect == TargetEffect.Other && CheckDefence();
        if (newCard.effectAction.duration > 0 && !blocked)
        {
            if (effects.ContainsKey(newCard.id))
                effects[newCard.id].PowerUp(effect);
            else
                effects.Add(newCard.id, effect);

            effects[newCard.id].BeginAction(GetCharacters, finishedCast, fastCast);
        }
        else
            effect.BeginAction(GetCharacters, finishedCast, fastCast);
    }

    public void AsyncTick(Action endTick, TypeEffect typeEffect)
    {
        CheckTicks(typeEffect);
        AsyncUseTick(AsyncCheckEndTicks, endTick);
    }

    public bool CheckDefence()
    {
        return effects.Any(eff => eff.Value.typeEffect == TypeEffect.Shield);
    }

    public bool UseDefence(ref int damage)
    {
        if (effects.Any(eff => eff.Value.typeEffect == TypeEffect.Shield))
        {
            var shield = effects.FirstOrDefault(eff => eff.Value.typeEffect == TypeEffect.Shield).Value;
            shield.UseEffect(() => { });
            CheckFinishedTicks();
            DropEffects();

            return true;
        }

        return false;
    }

    private void AsyncCheckEndTicks(Action endTick)
    {
        CheckFinishedTicks();
        AsyncUseTick(FinishTick, endTick);
    }

    private void AsyncUseTick(Action<Action> nextTick, Action endTick)
    {
        if (usingTicks.Count == 0)
        {
            nextTick?.Invoke(endTick);
            return;
        }

        usingTicks.Dequeue()(() => AsyncUseTick(nextTick, endTick));
    }

    private void CallEndTick(Action endTick)
    {
        endTick?.Invoke();
    }

    private void DropEffects()
    {
        foreach(CardID card in dropEffects)
            effects.Remove(card);

        dropEffects.Clear();
    }

    private void FinishTick(Action endTick)
    {
        selfCharacter.SetAnimatorActive(true);
        DropEffects();
        CallEndTick(endTick);
    }

    private void CheckTicks(TypeEffect typeEffect)
    {
        usingTicks.Clear();
        foreach (Effect effect in effects.Values)
        {
            if (effect.typeEffect != typeEffect) continue;

            usingTicks.Enqueue(effect.RoundAction);
        }
    }

    private void CheckFinishedTicks()
    {
        usingTicks.Clear();
        foreach (Effect effect in effects.Values)
        {
            //if (effect.typeEffect != TypeEffect.Attack) return;

            if (effect.CheckEnded)
            {
                usingTicks.Enqueue(effect.EndAction);
                dropEffects.Add(effect.ID);
            }
        }
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
