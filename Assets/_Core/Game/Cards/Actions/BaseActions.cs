using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActions : ScriptableObject, ICardAction
{
    public BaseCharacter.TypeAttack nameAnimation;
    public int duration = 0;
    public TypeEffect typeEffect;
    public TargetEffect target;
    public event Action OnFinishedCast;

    public BaseCharacter.TypeAttack NameAnimation => nameAnimation;

    public TypeEffect TypeEffect => typeEffect;

    public TargetEffect TargetEffect => target;

    public int Duration => duration;

    public abstract void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast);

    public abstract void End(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner);

    public abstract void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast);

    public virtual void PowerUp(Effect owner) { }

    protected void FinishedCast()
    {
        OnFinishedCast?.Invoke();
        OnFinishedCast = null;
    }
}
