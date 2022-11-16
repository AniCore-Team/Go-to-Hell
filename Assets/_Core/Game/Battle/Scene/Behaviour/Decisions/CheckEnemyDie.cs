using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Battle/Decisions/CheckEnemyDie", fileName = "CheckEnemyDie", order = 52)]
public class CheckEnemyDie : BattleDecisions
{
    public override bool GetDecision(BattleManager entity)
    {
        return entity.GetEnemyStateData().enemy.IsDie;
    }
}
