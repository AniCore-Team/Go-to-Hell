using System;

public interface ICardAction
{
    public event Action OnFinishedCast;

    public string NameAnimation { get; }
    public TypeEffect TypeEffect { get; }
    public TargetEffect TargetEffect { get; }
    public int Duration { get; }

    public void Cast(Effect owner, BaseCharacter self, BaseCharacter[] other, Action finishedCast);
    public void Tick(Effect owner, BaseCharacter self, BaseCharacter[] other);
    public void End(Action endTick, Effect owner);
}
