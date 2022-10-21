using Common;
using PureAnimator;
using System;
using System.Net;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "IcePrison", menuName = "Cards/Actions/IcePrison")]
public class IcePrisonAction : BaseActions
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
    public float powerUpDamageMultiplicer = 1.5f;
    public ParticleSystem particleSystemPrefab;
    public ParticleSystem explosionPrefab;
    public GameObject icePrefab;

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
            spawnPoint = obj.transform.GetChild(0).position;
            Destroy(obj);
        }

        owner.ClearLongTimeObjects();
        AsyncCastExplosion(endTick, spawnPoint);
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        finishedCast();
    }

    private void AsyncCastExplosion(Action endTick, Vector3 endPoint)
    {
        var explosion = Object.Instantiate(explosionPrefab, endPoint, Quaternion.identity);
        PureAnimation.Play(explosion.main.startLifetime.constant, progress =>
            {
                return default;
            }, () =>
            {
                Destroy(explosion.gameObject);
                endTick?.Invoke();
            });
    }

    private void AsyncMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        #region GetMoveData
        var effect = Object.Instantiate(particleSystemPrefab, castData.self.frontEffectSpawn.position, Quaternion.identity);

        var dist = Vector3.Distance(castData.self.transform.position, castData.other.transform.position);
        var shortDist = Vector3.Distance(
            new Vector3(castData.self.transform.position.x, 0, castData.self.transform.position.z),
            new Vector3(castData.self.frontEffectSpawn.position.x, 0, castData.self.frontEffectSpawn.position.z)
            );
        var endPoint = castData.self.frontEffectSpawn.position + castData.self.transform.forward * (dist - shortDist);

        castData.effect = effect.gameObject;
        castData.endMove = endPoint;
        #endregion GetMoveData

        PureAnimation.Play(dist / speed,
            progress =>
            {
                effect.transform.position = Vector3.Lerp(
                    castData.self.frontEffectSpawn.position,
                    endPoint,
                    progress);
                return default;
            },
            () => EndMoveEffectAnimation(castData, finishedCast) );
    }

    private void AsyncExplosionAnimation(CastData castData, Action finishedCast)
    {
        if (!castData.other.CardEffectsController.ContainsLongTimeObjects(CardID.IcePrison))
        {
            var startPoint = castData.other.transform.position + Vector3.down * 5f;
            var endPoint = castData.other.transform.position;
            var ice = Object.Instantiate(icePrefab, startPoint, Quaternion.identity);
            castData.owner.AddLongTimeObjects(ice);

            PureAnimation.Play(1f,
                progress =>
                {
                    ice.transform.position = endPoint;
                    return default;
                },
                () => EndExplosionAnimation(castData, finishedCast));
        }
        else
            finishedCast();
    }

    private void EndMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        Destroy(castData.effect.gameObject);
        AsyncCastExplosion(default, castData.endMove);

        bool isDefended = castData.other.CardEffectsController.CheckDefence();
        castData.other.Damage(castData.owner.isPowered ? (int)(damage * powerUpDamageMultiplicer) : damage);

        if (isDefended)
        {
            finishedCast?.Invoke();
            return;
        }

        PureAnimation.Play(0.1f,
            progress => default,
            () => {
                float delay = castData.other.GetLegthAnimation() - 0.1f;
                PureAnimation.Play(delay / 4f,
                    progress => default,
                    () => castData.other.SetAnimatorActive(false) );
            });

        AsyncExplosionAnimation(castData, finishedCast);
    }

    private void EndExplosionAnimation(CastData castData, Action finishedCast)
    {
        finishedCast?.Invoke();
    }
}
