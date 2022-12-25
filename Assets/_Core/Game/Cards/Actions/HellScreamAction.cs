using Common;
using PureAnimator;
using System;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "HellScream", menuName = "Cards/Actions/HellScream")]
public class HellScreamAction : BaseActions
{
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
            spawnPoint = obj.transform.position;
            obj.AddComponent<Rigidbody>();
            Destroy(obj, 3f);
        }

        owner.ClearLongTimeObjects();
        AsyncCastExplosion(endTick, spawnPoint);
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        finishedCast();
    }

    public override void PowerUp(Effect owner)
    {
        base.PowerUp(owner);
        owner.powerEffect += damage;
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
        var effect = Object.Instantiate(particleSystemPrefab, castData.self.transform.position, Quaternion.identity);

        castData.effect = effect.gameObject;
        #endregion GetMoveData

        PureAnimation.Play(effect.main.duration,
            Utils.EmptyPureAnimation,
            () => EndMoveEffectAnimation(castData, finishedCast) );
    }

    private void AsyncExplosionAnimation(CastData castData, Action finishedCast)
    {
        if (!castData.self.CardEffectsController.ContainsLongTimeObjects(CardID.HellScream))
        {
            var startPoint = castData.self.transform.position;
            var endPoint = castData.self.transform.position;
            var ice = Object.Instantiate(icePrefab, startPoint, Quaternion.identity);
            castData.owner.AddLongTimeObjects(ice);
            castData.owner.powerEffect = damage;

            PureAnimation.Play(1f, Utils.EmptyPureAnimation,
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
