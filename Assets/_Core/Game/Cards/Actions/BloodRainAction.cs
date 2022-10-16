using Common;
using PureAnimator;
using System;
using System.Net;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "BloodRain", menuName = "Cards/Actions/BloodRain")]
public class BloodRainAction : BaseActions
{
    private class CastData
    {
        public Effect owner;
        public BaseCharacter self;
        public BaseCharacter other;
        public GameObject effect;
        public Vector3 endMove;
    }


    public int damage;
    public float speed;
    public ParticleSystem bloodRainPrefab;
    public ParticleSystem bloodCloudfPrefab;
    public ParticleSystem bloodExplosionPrefab;

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
        Vector3 spawnPoint = Vector3.zero;
        foreach (var obj in owner.GetLongTimeObjects())
        {
            Destroy(obj);
        }

        owner.ClearLongTimeObjects();
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        other[0].Damage(damage);

        AsyncCastExplosion(finishedCast, other[0], other[0].transform.position + Vector3.up * 1.5f);
    }

    private void AsyncCastExplosion(Action endTick, BaseCharacter other, Vector3 endPoint)
    {
        var explosion = Object.Instantiate(bloodExplosionPrefab, endPoint, Quaternion.identity);
        PureAnimation.Play(0.1f,
            progress => default,
            () =>
            {
                float delay = other.GetLegthAnimation() - 0.1f;
                PureAnimation.Play(delay,
                    progress => default,
                    () =>
                    {
                        Destroy(explosion.gameObject);
                        endTick?.Invoke();
                    });
            });
    }

    private void AsyncMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        #region GetMoveData
        var effect = Object.Instantiate(bloodRainPrefab, castData.other.topEffectSpawn.position, Quaternion.identity);

        var endPoint = castData.other.topEffectSpawn.position;

        castData.effect = effect.gameObject;
        castData.endMove = endPoint;
        #endregion GetMoveData

        PureAnimation.Play(2f,
            progress => default,
            () => EndMoveEffectAnimation(castData, finishedCast) );
    }

    private void AsyncExplosionAnimation(CastData castData, Action finishedCast)
    {
        if (!castData.other.CardEffectsController.ContainsLongTimeObjects(CardID.BloodRain))
        {
            var startPoint = castData.other.transform.position + Vector3.up * 1.5f;
            var ice = Object.Instantiate(bloodCloudfPrefab, startPoint, Quaternion.identity);
            castData.owner.AddLongTimeObjects(ice.gameObject);

            PureAnimation.Play(1f,
                progress => default,
                () => EndExplosionAnimation(castData, finishedCast));
        }
        else
            finishedCast();
    }

    private void EndMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        Destroy(castData.effect.gameObject);

        AsyncExplosionAnimation(castData, finishedCast);
    }

    private void EndExplosionAnimation(CastData castData, Action finishedCast)
    {
        finishedCast?.Invoke();
    }
}
