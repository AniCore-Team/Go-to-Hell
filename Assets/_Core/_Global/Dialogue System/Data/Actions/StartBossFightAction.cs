using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Actions/StartBossFight", fileName = "StartBossFight", order = 52)]
public class StartBossFightAction : DialogueAction
{
    public override void BeginAction(DialogueWindow entity)
    {
        base.BeginAction(entity);
        entity.CloseDialogue();
    }
}
