using Common;
using PureAnimator;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Stun", menuName = "Cards/Actions/Stun")]
public class StunAction : BaseActions
{
    private class CastData
    {
        public Effect owner;
        public BaseCharacter self;
        public BaseCharacter other;
        public GameObject effect;
        public Vector3 endMove;
    }

    public GameObject stunPrefab;

    private PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        var castData = new CastData
        {
            owner = owner,
            self = self,
            other = other[0]
        };
        if (!castData.self.CardEffectsController.ContainsLongTimeObjects(CardID.Stun))
            AsyncMoveEffectAnimation(castData, finishedCast);
    }

    public override void End(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner)
    {
        foreach (var obj in owner.GetLongTimeObjects())
        {
            Destroy(obj);
        }

        owner.ClearLongTimeObjects();
        endTick?.Invoke();
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        finishedCast();
    }

    //public override bool Use(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner)
    //{
    //    base.Use(endTick, self, other, owner);
    //    End(endTick, self, other, owner);

    //    return true;
    //}

    private void AsyncMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        #region GetMoveData
        var effect = Object.Instantiate(stunPrefab, castData.other.topEffectSpawn.position, Quaternion.identity);
        castData.owner.AddLongTimeObjects(effect);
        castData.effect = effect.gameObject;
        #endregion GetMoveData

        var renders = effect.GetComponentsInChildren<Renderer>();
        PureAnimation.Play(1f, Utils.EmptyPureAnimation,
            () => AsyncExplosionAnimation(castData, finishedCast));
    }

    private void AsyncExplosionAnimation(CastData castData, Action finishedCast)
    {
        finishedCast();
    }
}
