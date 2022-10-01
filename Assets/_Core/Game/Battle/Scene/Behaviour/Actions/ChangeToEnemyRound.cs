using Common;
using PureAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleManager;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/ChangeToEnemyRound", fileName = "ChangeToEnemyRound", order = 52)]
public class ChangeToEnemyRound : BattleAction
{
    private PrepareEnemyStateData data;

    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        data = entity.GetPrepareEnemyStateData();
        entity.StartCoroutine(EndRound(entity));
    }

    public IEnumerator EndRound(BattleManager entity)
    {
        bool nextStep = false;

        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(1f, progress =>
            {
                data.battleWindow.HideUnlockedCard(1 - progress);
                return default;
            }, () =>
            {
                data.battleWindow.UnlockedAndClearCards();
                data.battleWindow.HidePanel(() => { nextStep = true; });
            });

        yield return new WaitUntil(() => nextStep);
        entity.stateRound = StateRound.Enemy;
    }
}
