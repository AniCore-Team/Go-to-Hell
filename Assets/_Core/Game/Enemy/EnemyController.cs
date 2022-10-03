using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Image cardAttackImage;
    [SerializeField] private EnemyProperty enemyProperty;
    private Animator animator;
    private CardProperty nextAttack;
    private int roundCounter = 0;

    private readonly int AttackId = Animator.StringToHash("Attack");

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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

        SetNextAttack(enemyProperty.simpleCards[Random.Range(0, enemyProperty.simpleCards.Count)]);
    }

    public void Attack()
    {
        animator.SetTrigger(AttackId);
    }

    public float GetLegthAnimation()
    {
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(currentClipInfo[0].clip.name);
        return currentClipInfo[0].clip.length;
    }

    private void SetNextAttack(CardProperty card)
    {
        nextAttack = card;
        cardAttackImage.sprite = card.icon;
    }
}
