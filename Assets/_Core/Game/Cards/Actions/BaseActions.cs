using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActions : ScriptableObject, ICardAction
{
    public string nameAnimation;
    public event Action OnFinishedCast;

    public string NameAnimation => nameAnimation;

    public abstract void Cast(BaseCharacter self, BaseCharacter[] other);

    public abstract void End();

    public abstract void Tick(BaseCharacter self, BaseCharacter other);

    protected void FinishedCast()
    {
        OnFinishedCast?.Invoke();
        OnFinishedCast = null;
    }
}
