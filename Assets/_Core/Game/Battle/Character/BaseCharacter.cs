using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public enum TypeAttack
    {
        Fireball
    }

    protected Animator animator;
    public Transform frontEffectSpawn;
    public Transform topEffectSpawn;
    public Transform summonPointSpawn;

    public CharacterHUD characterHUD;
    private CardEffectsController cardEffectsController;
    protected StateRound nextRound;

    private readonly int HitId = Animator.StringToHash("Hit");
    private readonly int TypeAttackId = Animator.StringToHash("TypeAttack");
    private readonly int AttackId = Animator.StringToHash("Attack");

    private int health = 100;
    private int maxHealth = 100;
    public bool isStun = false;

    public CardEffectsController CardEffectsController => cardEffectsController;
    public StateRound NextRound => nextRound;

    public virtual void Init(CinemachineSwitcher cinemachineSwitcher)
    {
        animator = GetComponentInChildren<Animator>();

        var baseControllers = FindObjectsOfType<BaseCharacter>();
        cardEffectsController = new CardEffectsController();
        cardEffectsController.Init(this, baseControllers.FirstOrDefault(c => c != this), cinemachineSwitcher);
    }

    public void Damage(int damage)
    {
        if (CardEffectsController.UseDefence(ref damage))
            return;

        health -= damage;
        characterHUD.SetHealth(health / (float)maxHealth);
        if (animator.enabled)
            animator.SetTrigger(HitId);

        if (health <= 0)
            if (this is EnemyController)
                Translator.Send(InnerProtocol.WinBattle);
            else
                Translator.Send(InnerProtocol.LoseBattle);
    }

    public void Heal(int heal)
    {
        health += heal;
        characterHUD.SetHealth(health / (float)maxHealth);

        if (health >= maxHealth)
            health = maxHealth;
    }

    public void DefenceUp(int value)
    {

    }

    public void DefenceDown(int value)
    {

    }

    public virtual void Attack(TypeAttack typeAttack)
    {
        if (animator.enabled)
        {
            animator.SetInteger(TypeAttackId, (int)typeAttack);
            animator.SetTrigger(AttackId);
        }
    }

    public void SetAnimatorActive(bool isActive)
    {
        animator.enabled = isActive;
    }

    public float GetLegthAnimation()
    {
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(currentClipInfo[0].clip.name);
        return currentClipInfo[0].clip.length;
    }

    public float[] GetEventTimeAnimation()
    {
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        var times = new float[currentClipInfo[0].clip.events.Length];
        for (var i = 0; i < currentClipInfo[0].clip.events.Length; i++)
        {
            times[i] = currentClipInfo[0].clip.events[i].time;
        }
        return times;
    }
}
