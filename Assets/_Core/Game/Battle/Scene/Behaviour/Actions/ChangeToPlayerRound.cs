using UnityEngine;
using Zenject;
using static BattleManager;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(menuName = "Behaviour/Battle/Actions/ChangeToPlayerRound", fileName = "NewChangeToPlayerRoundAction", order = 52)]
public class ChangeToPlayerRound : BattleAction
{
    private PreparePlayerStateData dataPlayer;
    private EnemyStateData dataEnemy;
    private BattleManager entity;

    public override void BeginAction(BattleManager entity)
    {
        base.BeginAction(entity);
        this.entity = entity;
        dataPlayer = entity.GetPreparePlayerStateData();
        dataEnemy = entity.GetEnemyStateData();

        AsyncTickAttack();
    }

    private void AsyncTickAttack()
    {
        dataPlayer.player.CardEffectsController.AsyncTick(AsyncTickShield, TypeEffect.Attack);
    }

    private void AsyncTickShield()
    {
        dataPlayer.player.CardEffectsController.AsyncTick(AsyncTickStun, TypeEffect.Shield);
    }

    private void AsyncTickStun()
    {
        dataPlayer.player.isStun = dataPlayer.player.CardEffectsController.IsStun;
        dataPlayer.player.CardEffectsController.AsyncTick(EndTick, TypeEffect.Stun);
    }

    private void EndTick()
    {
        Scoring();

        if (dataPlayer.player.isStun)
            entity.StateRound = StateRound.Player;
        else
            dataPlayer.battleWindow.ShowPanel(AsyncIssuanceCards);
    }

    public void AsyncIssuanceCards()
    {
        var count = dataPlayer.battleWindow.GetCountFreeSlots();
        CardView[] newCards = new CardView[count];
        for (int i = 0; i < count; i++)
        {
            var randomCard = dataPlayer.deck.GetRandomCard();
            newCards[i] = dataPlayer.factory.Create(randomCard.card.prefab);
            newCards[i].Property = randomCard;
            newCards[i].gameObject.name += i.ToString();
        }
        dataPlayer.battleWindow.SetCard(() => entity.StateRound = StateRound.Player,
            newCards);
    }

    private void Scoring()
    {
        dataEnemy.enemy.PreparingToAttack();
        entity.battlePoint += 10;
        dataPlayer.battleWindow.RepaintPointText(entity.battlePoint);
    }
}
