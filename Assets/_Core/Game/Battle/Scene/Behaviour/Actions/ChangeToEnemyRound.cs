using PureAnimator;
using System;
using UnityEngine;
using static BattleManager;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/ChangeToEnemyRound", fileName = "ChangeToEnemyRound", order = 52)]
public class ChangeToEnemyRound : BattleAction
{
    private PrepareEnemyStateData data;
    private BattleManager entity;

    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        this.entity = entity;
        data = entity.GetPrepareEnemyStateData();

        PureAnimation.Play(1f, progress =>
        {
            data.battleWindow.HideUnlockedCard(1 - progress);
            return default;
        }, () =>
        {
            data.battleWindow.UnlockedAndClearCards();
            data.battleWindow.HidePanel(() => AsyncTickAttack());
        });
    }

    private void AsyncTickAttack()
    {
        data.enemy.CardEffectsController.AsyncTick(AsyncTickShield, TypeEffect.Attack);
    }

    private void AsyncTickShield()
    {
        data.enemy.CardEffectsController.AsyncTick(AsyncTickStun, TypeEffect.Shield);
    }

    private void AsyncTickStun()
    {
        data.enemy.isStun = data.enemy.CardEffectsController.IsStun;
        data.enemy.CardEffectsController.AsyncTick(EndTick, TypeEffect.Stun);
    }

    private void EndTick()
    {
        entity.StateRound = StateRound.Enemy;
    }
}
