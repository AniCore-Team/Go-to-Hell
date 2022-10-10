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
        if (data.player.isStun)
            data.OnNextRound?.Invoke(StateRound.PreEnemy);
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


                data.battleWindow.SetActiveBottomPanel(false);
                entity.enabled = false;
                //data.player.CardEffectsController.AddEffect(data.currentCard.property.card, CastControl);
                switch (data.enemy.CurrentCard.target)
                {
                    case TargetEffect.All:
                        data.enemy.CardEffectsController.AddEffect(data.currentCard.property.card, CastControl);
                        data.player.CardEffectsController.AddEffect(data.currentCard.property.card, CastControl);
                        break;
                    case TargetEffect.Self:
                        data.player.CardEffectsController.AddEffect(data.currentCard.property.card, CastControl);
                        break;
                    case TargetEffect.Other:
                        data.enemy.CardEffectsController.AddEffect(data.currentCard.property.card, CastControl);
                        break;
                }

                data.currentCard.Use(data.battleWindow.ShiftToFreeSlots);
                entity.battlePoint -= data.currentCard.property.card.cost;
                data.battleWindow.RepaintPointText(entity.battlePoint);
            }
        }
    }

    private void CastControl()
    {
        data.battleWindow.SetActiveBottomPanel(true);
        data.battleManager.enabled = true;
    }
}
