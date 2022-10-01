using UnityEngine;
using Zenject;
using static BattleManager;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/ChangeToPlayerRound", fileName = "NewChangeToPlayerRoundAction", order = 52)]
public class ChangeToPlayerRound : BattleAction
{
    private PreparePlayerStateData data;
    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        data = entity.GetPreparePlayerStateData();
        StartRound(entity);
    }

    public void StartRound(BattleManager entity)
    {
        entity.battlePoint += 10;
        data.battleWindow.SetPointText(entity.battlePoint);
        var count = data.battleWindow.GetCountFreeSlots();

        CardView[] newCards = new CardView[count];
        for (int i = 0; i < count; i++)
        {
            var randomCard = data.deck.GetRandomCard();
            newCards[i] = data.factory.Create(randomCard.card.prefab);
            newCards[i].property = randomCard;
            newCards[i].gameObject.name += i.ToString();
        }
        data.battleWindow.SetCard(newCards);
        entity.stateRound = StateRound.Player;
    }
}
