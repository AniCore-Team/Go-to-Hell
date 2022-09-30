using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/PlayerRound", fileName = "NewPlayerRoundAction", order = 52)]
public class PlayerRound : BattleAction
{
    public override void DoAction(BattleManager entity)
    {
        base.DoAction(entity);
        entity.UpdateRound();
    }
}
