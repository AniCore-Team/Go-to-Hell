using Common;
using PureAnimator;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "EdemAngel", menuName = "Cards/Actions/EdemAngel")]
public class EdemAngelAction : BaseActions
{
    public GameObject shieldPrefab;
    public int durability;
    public CardProperty bonusEffect;

    private PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        var castData = new CastData
        {
            owner = owner,
            self = self,
            other = other[0]
        };
        if (self.CardEffectsController.ContainsLongTimeObjects(CardID.Shield))
        {
            int d = 0;
            self.CardEffectsController.UseDefence(ref d);
        }

        if (!castData.self.CardEffectsController.ContainsLongTimeObjects(CardID.EdemAngel))
        {
            AsyncMoveEffectAnimation(castData, finishedCast);
            owner.powerEffect = durability;
        }
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
        if (owner.powerEffect > 0)
            self.CardEffectsController.AddEffect(bonusEffect, endTick, true);
        else
            endTick?.Invoke();
    }

    public override void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast)
    {
        finishedCast();
    }

    public override bool Use(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner)
    {
        base.Use(endTick, self, other, owner);
        if (owner.powerEffect > durability)
            owner.powerEffect = durability;
        owner.powerEffect--;

        if (owner.powerEffect == 0)
        {
            End(endTick, self, other, owner);
            return true;
        }

        return false;
    }

    public override void PowerUp(Effect owner)
    {
        base.PowerUp(owner);
        owner.duration++;
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
