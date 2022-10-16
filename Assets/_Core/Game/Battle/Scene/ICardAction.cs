using System;

public interface ICardAction
{
    public event Action OnFinishedCast;

    public BaseCharacter.TypeAttack NameAnimation { get; }
    public TypeEffect TypeEffect { get; }
    public TargetEffect TargetEffect { get; }
    public int Duration { get; }

    public void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast);
    public void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast);
    public void End(Action endTick, BaseCharacter self, BaseCharacter[] other, Effect owner);
}
