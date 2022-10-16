using Common;
using PureAnimator;
using System;
using System.Net;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "EdemApple", menuName = "Cards/Actions/EdemApple")]
public class EdemAppleAction : BaseActions
{
    private class CastData
    {
        public Effect owner;
        public BaseCharacter self;
        public BaseCharacter other;
        public GameObject effect;
        public Vector3 endMove;
    }

    public int health;
    public ParticleSystem particleSystemPrefab;

    private PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        var castData = new CastData
        {
            owner = owner,
            self = self,
            other = other[0]
        };
        AsyncMoveEffectAnimation(castData, finishedCast);
    }

    public override void End(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner)
    {
        endTick?.Invoke();
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast) { }

    private void AsyncMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        #region GetMoveData
        var effect = Object.Instantiate(particleSystemPrefab, castData.self.transform.position, Quaternion.identity);

        castData.effect = effect.gameObject;
        #endregion GetMoveData

        PureAnimation.Play(effect.main.duration + effect.main.startLifetime.constant,
            progress => default,
            () => EndMoveEffectAnimation(castData, finishedCast) );
    }

    private void EndMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        Destroy(castData.effect.gameObject);
        castData.self.Heal(health);

        finishedCast?.Invoke();
    }
}
