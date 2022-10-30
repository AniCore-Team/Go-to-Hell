using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActions : ScriptableObject, ICardAction
{
    protected class CastData
    {
        public Effect owner;
        public BaseCharacter self;
        public BaseCharacter other;
        public GameObject effect;
        public Vector3 endMove;
    }

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

    public virtual bool Use(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner) => false;

    public virtual void PowerUp(Effect owner) { }

    protected void Damage(CastData castData, int damage)
    {
        int newDamage = castData.self.CardEffectsController.IsDebuff ? damage / 2 : damage;
        newDamage = castData.self.CardEffectsController.IsBuff ?
            newDamage * castData.self.CardEffectsController.GetEffect(CardID.HellScream).powerEffect :
            newDamage;
        castData.other.Damage(newDamage);
    }
}
