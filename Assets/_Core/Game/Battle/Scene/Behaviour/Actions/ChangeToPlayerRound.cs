using UnityEngine;
using Zenject;
using static BattleManager;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/ChangeToPlayerRound", fileName = "NewChangeToPlayerRoundAction", order = 52)]
public class ChangeToPlayerRound : BattleAction
{
    private PreparePlayerStateData dataPlayer;
    private EnemyStateData dataEnemy;
    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        dataPlayer = entity.GetPreparePlayerStateData();
        dataEnemy = entity.GetEnemyStateData();
        dataPlayer.battleWindow.ShowPanel(() => { StartRound(entity); });
    }

    public void StartRound(BattleManager entity)
    {
        dataEnemy.enemy.PreparingToAttack();
        entity.battlePoint += 10;
        dataPlayer.battleWindow.SetPointText(entity.battlePoint);
        var count = dataPlayer.battleWindow.GetCountFreeSlots();

        CardView[] newCards = new CardView[count];
        for (int i = 0; i < count; i++)
        {
            var randomCard = dataPlayer.deck.GetRandomCard();
            newCards[i] = dataPlayer.factory.Create(randomCard.card.prefab);
            newCards[i].property = randomCard;
            newCards[i].gameObject.name += i.ToString();
        }
        dataPlayer.battleWindow.SetCard(() => { entity.StateRound = StateRound.Player; },
            newCards);
    }
}
