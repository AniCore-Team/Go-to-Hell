using Common;
using PureAnimator;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/EnemyRound", fileName = "EnemyRound", order = 52)]
public class EnemyRound : BattleAction
{
    public override void DoAction(BattleManager entity)
    {
        base.DoAction(entity);
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(1f, progress =>
            {
                return default;
            }, () =>
            {
                entity.stateRound = StateRound.PrePlayer;
            });
    }
}
