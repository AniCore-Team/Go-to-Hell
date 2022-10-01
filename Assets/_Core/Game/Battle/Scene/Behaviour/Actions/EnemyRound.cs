using Common;
using PureAnimator;
using UnityEngine;
using static BattleManager;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/EnemyRound", fileName = "EnemyRound", order = 52)]
public class EnemyRound : BattleAction
{
    private PrepareEnemyStateData data;

    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        data = entity.GetPrepareEnemyStateData();
        data.enemy.Attack();
    }

    public override void DoAction(BattleManager entity)
    {
        base.DoAction(entity);
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(0.1f, progress =>
            {
                return default;
            }, () =>
            {
                Services<PureAnimatorController>
                    .Get()
                    .GetPureAnimator()
                    .Play(data.enemy.GetLegthAnimation(), progress =>
                    {
                        return default;
                    }, () =>
                    {
                        entity.stateRound = StateRound.PrePlayer;
                    });
            });
    }
}
