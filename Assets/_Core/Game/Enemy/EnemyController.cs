using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;

    private readonly int AttackId = Animator.StringToHash("Attack");

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger(AttackId);
    }
}
