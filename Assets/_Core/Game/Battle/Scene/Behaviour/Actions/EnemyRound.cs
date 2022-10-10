using Common;
using PureAnimator;
using UnityEngine;
using static BattleManager;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/EnemyRound", fileName = "EnemyRound", order = 52)]
public class EnemyRound : BattleAction
{
    private EnemyStateData data;

    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        data = entity.GetEnemyStateData();

        if (data.enemy.isStun)
            data.OnNextRound?.Invoke(StateRound.PrePlayer);

        switch (data.enemy.CurrentCard.target)
        {
            case TargetEffect.All:
                data.enemy.CardEffectsController.AddEffect(data.enemy.CurrentCard, CastControl);
                data.player.CardEffectsController.AddEffect(data.enemy.CurrentCard, CastControl);
                break;
            case TargetEffect.Self:
                data.enemy.CardEffectsController.AddEffect(data.enemy.CurrentCard, CastControl);
                break;
            case TargetEffect.Other:
                data.player.CardEffectsController.AddEffect(data.enemy.CurrentCard, CastControl);
                break;
        }
    }

    private void CastControl()
    {
        data.OnNextRound?.Invoke(StateRound.PrePlayer);
    }
}
