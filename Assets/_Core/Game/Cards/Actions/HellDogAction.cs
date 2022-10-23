using Common;
using PureAnimator;
using System;
using System.Net;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "HellDog", menuName = "Cards/Actions/HellDog")]
public class HellDogAction : BaseActions
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
    public ParticleSystem spawnEffectPrefab;
    public GameObject hellDogPrefab;
    public ParticleSystem attackExplosionPrefab;

    private PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        var castData = new CastData
        {
            owner = owner,
            self = self,
            other = other[0]
        };
        AsyncSpawnEffect(castData, finishedCast);
    }

    public override void End(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner)
    {
        Vector3 spawnPoint = Vector3.zero;
        foreach (var obj in owner.GetLongTimeObjects())
        {
            Destroy(obj);
        }

        owner.ClearLongTimeObjects();
        AsyncCastExplosion(self.summonPointSpawn.position, default);
        endTick?.Invoke();
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        //other[0].Damage(damage);
        var castData = new CastData
        {
            owner = owner,
            self = self,
            other = other[0],
            effect = owner.GetLongTimeObjects()[0]
        };

        AsyncMoveEffectAnimation(castData, finishedCast);
    }

    public override void PowerUp(Effect owner)
    {
        base.PowerUp(owner);
        owner.duration++;
    }

    private void AsyncSpawnEffect(CastData castData, Action finishedCast)
    {
        var effect = Object.Instantiate(hellDogPrefab, castData.self.summonPointSpawn.position, Quaternion.identity);
        castData.owner.AddLongTimeObjects(effect);

        AsyncCastExplosion(effect.transform.position, finishedCast);
    }

    private void AsyncCastExplosion(Vector3 endPoint, Action finishedCast)
    {
        var explosion = Object.Instantiate(spawnEffectPrefab, endPoint, Quaternion.identity);

        PureAnimation.Play(explosion.main.duration + explosion.main.startLifetime.constant,
            progress => default,
            () =>
            {
                Destroy(explosion.gameObject);
                finishedCast?.Invoke();
            });
    }

    private void AsyncMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        #region GetMoveData
        var dist = Vector3.Distance(castData.effect.transform.position, castData.other.transform.position) - 2f;
        var direction = castData.other.transform.position - castData.effect.transform.position;
        var endPoint = castData.effect.transform.position + direction.normalized * dist;

        castData.endMove = endPoint;
        #endregion GetMoveData

        PureAnimation.Play(1f,
            progress =>
            {
                castData.effect.transform.position = Vector3.Lerp(
                    castData.self.summonPointSpawn.position,
                    endPoint,
                    progress);
                return default;
            },
            () => EndMoveEffectAnimation(castData, finishedCast) );
    }

    private void AsyncReturnMoveAnimation(CastData castData, Action finishedCast)
    {
        #region GetMoveData
        var dist = Vector3.Distance(castData.effect.transform.position, castData.other.transform.position) - 2f;
        var direction = castData.self.summonPointSpawn.position - castData.effect.transform.position;
        var endPoint = castData.effect.transform.position + direction.normalized * dist;

        castData.endMove = endPoint;
        #endregion GetMoveData

        PureAnimation.Play(1f,
            progress =>
            {
                castData.effect.transform.position = Vector3.Lerp(
                    endPoint,
                    castData.self.summonPointSpawn.position,
                    progress);
                return default;
            },
            () => EndExplosionAnimation(castData, finishedCast));
    }

    //private void AsyncExplosionAnimation(CastData castData, Action finishedCast)
    //{
    //    if (!castData.other.CardEffectsController.ContainsLongTimeObjects(CardID.BloodRain))
    //    {
    //        var startPoint = castData.other.transform.position + Vector3.up * 1.5f;
    //        var ice = Object.Instantiate(hellDogPrefab, startPoint, Quaternion.identity);
    //        castData.owner.AddLongTimeObjects(ice.gameObject);

    //        PureAnimation.Play(1f,
    //            progress => default,
    //            () => EndExplosionAnimation(castData, finishedCast));
    //    }
    //    else
    //        finishedCast();
    //}

    private void EndMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        castData.other.Damage(damage);
        AsyncWaitAnimationEvent(castData, finishedCast);
    }

    private void EndExplosionAnimation(CastData castData, Action finishedCast)
    {
        finishedCast?.Invoke();
    }

    private void AsyncWaitAnimationEvent(CastData castData, Action finishedCast)
    {
        castData.effect.GetComponent<Animator>().SetTrigger("Attack");
        PureAnimation.Play(0.1f, Utils.EmptyPureAnimation, () =>
        {
            //var allTimeAnimation = getCharacter(TargetEffect.Self)[0].GetLegthAnimation();
            var eventTimeAnimations = GetEventTimeAnimation(castData);

            for (var i = 0; i < eventTimeAnimations.Length; i++)
            {
                PureAnimation.Play(eventTimeAnimations[i], Utils.EmptyPureAnimation, () =>
                {
                    AsyncReturnMoveAnimation(castData, finishedCast);
                });
            }
        });
    }

    private float[] GetEventTimeAnimation(CastData castData)
    {
        //var currentClipInfo = castData.effect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        //var times = new float[currentClipInfo[0].clip.events.Length];
        //for (var i = 0; i < currentClipInfo[0].clip.events.Length; i++)
        //{
        //    times[i] = currentClipInfo[0].clip.events[i].time;
        //}
        return new float[] { 1f };
    }
}
