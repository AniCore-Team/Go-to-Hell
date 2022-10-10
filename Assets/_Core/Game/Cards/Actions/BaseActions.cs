using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActions : ScriptableObject, ICardAction
{
    public string nameAnimation;
    public int duration = 0;
    public TypeEffect typeEffect;
    public TargetEffect target;
    public event Action OnFinishedCast;

    public string NameAnimation => nameAnimation;

    public TypeEffect TypeEffect => typeEffect;

    public TargetEffect TargetEffect => target;

    public int Duration => duration;

    public abstract void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast);

    public abstract void End(Action endTick, Effect owner);

    public abstract void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other);

    protected void FinishedCast()
    {
        OnFinishedCast?.Invoke();
        OnFinishedCast = null;
    }
}
