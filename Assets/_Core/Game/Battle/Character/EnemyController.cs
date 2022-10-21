using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : BaseCharacter
{
    [SerializeField] private Image cardAttackImage;
    [SerializeField] private EnemyProperty enemyProperty;
    private CardProperty nextAttack;
    private int roundCounter = 0;

    public CardProperty CurrentCard => nextAttack;
    //private readonly int AttackId = Animator.StringToHash("Attack Faerball");

    public override void Init()
    {
        base.Init();
        nextRound = StateRound.PrePlayer;
    }

    public void PreparingToAttack()
    {
        roundCounter++;
        foreach (var hardCard in enemyProperty.hardAttacks)
        {
            if (roundCounter % hardCard.interval == 0)
                if (Random.value > hardCard.chance)
                {
                    SetNextAttack(hardCard.hardCards[Random.Range(0, hardCard.hardCards.Count)]);
                    return;
                }
        }

        if (enemyProperty.simpleCards.Count > 0)
            SetNextAttack(enemyProperty.simpleCards[Random.Range(0, enemyProperty.simpleCards.Count)]);
    }

    private void SetNextAttack(CardProperty card)
    {
        nextAttack = card;
        cardAttackImage.sprite = card.icon;
        cardAttackImage.gameObject.SetActive(true);
    }

    public void HideCardIcon()
    {
        cardAttackImage.gameObject.SetActive(false);
    }
}
