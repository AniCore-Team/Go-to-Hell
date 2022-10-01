using UnityEngine;
using static BattleManager;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/PlayerRound", fileName = "NewPlayerRoundAction", order = 52)]
public class PlayerRound : BattleAction
{
    private PlayerStateData data;

    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        data = entity.GetPlayerStateData();
    }

    public override void DoAction(BattleManager entity)
    {
        base.DoAction(entity);
        data.cardDetector.Update();
        if (data.cardDetector.isTarget)
        {
            data.currentCard = data.cardDetector.TargetObject;
            if (Input.GetMouseButtonUp(0))
            {
                if (entity.battlePoint < data.currentCard.property.card.cost) return;

                data.currentCard.Use(data.battleWindow.ShiftToFreeSlots);
                entity.battlePoint -= data.currentCard.property.card.cost;
                data.battleWindow.SetPointText(entity.battlePoint);
            }
        }
    }
}
