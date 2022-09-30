using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/ChangeToPlayerRound", fileName = "NewChangeToPlayerRoundAction", order = 52)]
public class ChangeToPlayerRound : BattleAction
{
    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        entity.StartRound();
    }
}
