using BehaviourSystem;
using Common;
using PureAnimator;

public class DialogueAction : BehaviourAction<LevelDialogueWindow>
{
    protected PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public override void BeginAction(LevelDialogueWindow entity)
    { }

    public override void DoAction(LevelDialogueWindow entity)
    { }

    public override void DoFixAction(LevelDialogueWindow entity)
    { }

    public override void DoLateAction(LevelDialogueWindow entity)
    { }

    public override void EndAction(LevelDialogueWindow entity)
    { }
}
