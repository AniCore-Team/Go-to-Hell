using Common;
using PureAnimator;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "JacksShadow", menuName = "Cards/Actions/JacksShadow")]
public class JacksShadowAction : BaseActions
{
    public int damage;
    [Range(0f, 1f)] public float thresholdStun;
    public CardProperty stunEffect;
    public ParticleSystem dustSpawnPrefab;
    public GameObject JacksShadowPrefab;
    public GameObject shadowSpikePrefab;
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
        endTick?.Invoke();
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        var castData = new CastData
        {
            owner = owner,
            self = self,
            other = other[0],
            effect = owner.GetLongTimeObjects()[0]
        };

        if (owner.CheckEnded)
            AsyncWaitLatestAnimationEvent(castData, finishedCast);
        else
            AsyncWaitAnimationEvent(castData, finishedCast);
    }

    public override void PowerUp(Effect owner)
    {
        base.PowerUp(owner);
        owner.duration++;
    }

    private void AsyncSpawnEffect(CastData castData, Action finishedCast)
    {
        if (!castData.other.CardEffectsController.ContainsLongTimeObjects(CardID.JacksShadow))
        {
            var effect = Object.Instantiate(JacksShadowPrefab, castData.other.transform.position + Vector3.right * 2f, Quaternion.identity);
            castData.owner.AddLongTimeObjects(effect);
            castData.effect = effect;

            AsyncCastExplosion(effect.transform.position, finishedCast);
        }
        else
            finishedCast?.Invoke();
    }

    private void AsyncCastExplosion(Vector3 endPoint, Action finishedCast)
    {
        var explosion = Object.Instantiate(dustSpawnPrefab, endPoint, Quaternion.identity);

        PureAnimation.Play(explosion.main.duration,
            progress => default,
            () =>
            {
                Destroy(explosion.gameObject);
                finishedCast?.Invoke();
            });
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
                    EndEffectAnimation(castData, finishedCast);
                });
            }
        });
    }

    private void AsyncWaitLatestAnimationEvent(CastData castData, Action finishedCast)
    {
        castData.effect.GetComponent<Animator>().SetTrigger("Attack");
        PureAnimation.Play(0.1f, Utils.EmptyPureAnimation, () =>
        {
            var eventTimeAnimations = GetEventTimeAnimation(castData);
            for (var i = 0; i < eventTimeAnimations.Length; i++)
                PureAnimation.Play(eventTimeAnimations[i], Utils.EmptyPureAnimation, () =>
                {
                    EndLatestEffectAnimation(castData, finishedCast);
                });
        });
    }

    private void AsyncExplosionAnimation(CastData castData, Action finishedCast)
    {
        var startPoint = castData.other.transform.position + Vector3.up * 1.5f;
        var blood = Object.Instantiate(bloodExplosionPrefab, startPoint, Quaternion.identity);

        PureAnimation.Play(blood.main.duration,
            Utils.EmptyPureAnimation,
            () => EndExplosionAnimation(castData, finishedCast));
    }

    private void AsyncSpikeAnimation(CastData castData, Action finishedCast)
    {
        var startPoint = castData.other.transform.position;
        var spike = Object.Instantiate(shadowSpikePrefab, startPoint, Quaternion.identity);

        PureAnimation.Play(1f, Utils.EmptyPureAnimation,
            () => {
                Destroy(spike);
                EndExplosionAnimation(castData, finishedCast);
            });
    }

    private void EndEffectAnimation(CastData castData, Action finishedCast)
    {
        //Destroy(castData.effect.gameObject);
        if (UnityEngine.Random.value > thresholdStun)
        {
            Damage(castData, damage);
            AsyncExplosionAnimation(castData, finishedCast);
        }
        else
        {
            //cast stun effect
            castData.other.Damage(0);
            castData.other.CardEffectsController.AddEffect(stunEffect, finishedCast, true);
        }
    }

    private void EndLatestEffectAnimation(CastData castData, Action finishedCast)
    {
        castData.other.Damage(damage);
        AsyncSpikeAnimation(castData, finishedCast);
    }

    private void EndExplosionAnimation(CastData castData, Action finishedCast)
    {
        finishedCast?.Invoke();
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
