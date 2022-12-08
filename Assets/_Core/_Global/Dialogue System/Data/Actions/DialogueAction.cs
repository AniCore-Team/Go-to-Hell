using BehaviourSystem;
using Common;
using PureAnimator;

public class DialogueAction : BehaviourAction<DialogueWindow>
{
    protected PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void BeginAction(DialogueWindow entity)
    { }

    public override void DoAction(DialogueWindow entity)
    { }

    public override void DoFixAction(DialogueWindow entity)
    { }

    public override void DoLateAction(DialogueWindow entity)
    { }

    public override void EndAction(DialogueWindow entity)
    { }
}
