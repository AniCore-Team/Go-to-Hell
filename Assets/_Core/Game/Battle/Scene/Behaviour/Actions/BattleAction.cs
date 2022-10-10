using BehaviourSystem;
using Common;
using PureAnimator;

public class BattleAction : BehaviourAction<BattleManager>
{
    protected PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void BeginAction(BattleManager entity)
    { }

    public override void DoAction(BattleManager entity)
    { }

    public override void DoFixAction(BattleManager entity)
    { }

    public override void DoLateAction(BattleManager entity)
    { }

    public override void EndAction(BattleManager entity)
    { }
}
