using Common;
using PureAnimator;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Shield", menuName = "Cards/Actions/Shield")]
public class ShieldAction : BaseActions
{
    private class CastData
    {
        public Effect owner;
        public BaseCharacter self;
        public BaseCharacter other;
        public GameObject effect;
        public Vector3 endMove;
    }

    public GameObject shieldPrefab;

    private PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        var castData = new CastData
        {
            owner = owner,
            self = self,
            other = other[0]
        };
        if (self.CardEffectsController.IsDebuff)
            finishedCast?.Invoke();
        else
            if (!castData.self.CardEffectsController.ContainsLongTimeObjects(CardID.Shield))
                if (castData.self.CardEffectsController.ContainsLongTimeObjects(CardID.EdemAngel))
                    castData.self.CardEffectsController.GetEffect(CardID.EdemAngel).powerEffect++;
                else
                    AsyncMoveEffectAnimation(castData, finishedCast);
    }

    public override void End(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner)
    {
        Vector3 spawnPoint = Vector3.zero;
        foreach (var obj in owner.GetLongTimeObjects())
        {
            spawnPoint = obj.transform.GetChild(0).position;
            obj.AddComponent<Rigidbody>();
            Destroy(obj, 2f);
        }

        owner.ClearLongTimeObjects();
        endTick?.Invoke();
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        owner.duration++;
        finishedCast();
    }

    public override bool Use(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner)
    {
        base.Use(endTick, self, other, owner);
        End(endTick, self, other, owner);

        return true;
    }

    private void AsyncMoveEffectAnimation(CastData castData, Action finishedCast)
    {
        #region GetMoveData
        var effect = Object.Instantiate(shieldPrefab, castData.self.transform.position, Quaternion.identity);
        castData.owner.AddLongTimeObjects(effect);
        castData.effect = effect.gameObject;
        #endregion GetMoveData

        var renders = effect.GetComponentsInChildren<Renderer>();
        PureAnimation.Play(1f,
            progress =>
            {
                foreach (var render in renders)
                {
                    Color color = render.material.color;
                    color.a = progress;
                    render.material.color = color;
                }
                return default;
            },
            () => AsyncExplosionAnimation(castData, finishedCast));
    }

    private void AsyncExplosionAnimation(CastData castData, Action finishedCast)
    {
        finishedCast();
    }
}
