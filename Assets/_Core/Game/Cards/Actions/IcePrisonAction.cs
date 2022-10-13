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

    public override void End(Action endTick, Effect owner)
    {
        Vector3 spawnPoint = Vector3.zero;
        foreach (var obj in owner.GetLongTimeObjects())
        {
            spawnPoint = obj.transform.GetChild(0).position;
            obj.AddComponent<Rigidbody>();
            Destroy(obj, 3f);
        }

        owner.ClearLongTimeObjects();
        AsyncCastExplosion(endTick, spawnPoint);
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other)
    {

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
                    ice.transform.position = Vector3.Lerp(
                        startPoint,
                        endPoint,
                        progress);
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
        castData.other.Damage(castData.owner.isPowered ? (int)(damage * 1.5f) : damage);

        AsyncCastExplosion(default, castData.endMove);
        AsyncExplosionAnimation(castData, finishedCast);
    }

    private void EndExplosionAnimation(CastData castData, Action finishedCast)
    {
        finishedCast?.Invoke();
    }
}
