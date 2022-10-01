using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Battle/Decisions/CheckBattleState", fileName = "NewCheckBattleStateDecisions", order = 52)]
public class CheckBattleState : BattleDecisions
{
    [SerializeField] private StateRound stateRound;
    public override bool GetDecision(BattleManager entity)
    {
        return entity.stateRound == stateRound;
    }
}
