using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    protected Animator animator;
    public Transform frontEffectSpawn;

    private CardEffectsController cardEffectsController;
    protected StateRound nextRound;

    private readonly int HitId = Animator.StringToHash("Hit");

    public CardEffectsController CardEffectsController => cardEffectsController;
    public StateRound NextRound => nextRound;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();

        var baseControllers = FindObjectsOfType<BaseCharacter>();
        cardEffectsController = new CardEffectsController();
        cardEffectsController.Init(this, baseControllers.FirstOrDefault(c => c != this));
    }

    public void Damage(int damage)
    {
        animator.Play(HitId);
    }

    public void Heal(int heal)
    {

    }

    public void DefenceUp(int value)
    {

    }

    public void DefenceDown(int value)
    {

    }

    public void Attack(string nameAnimation)
    {
        animator.Play(nameAnimation);
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
