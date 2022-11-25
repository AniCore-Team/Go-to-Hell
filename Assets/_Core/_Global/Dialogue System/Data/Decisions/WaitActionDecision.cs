using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Decisions/WaitAction", fileName = "WaitAction", order = 52)]
public class WaitActionDecision : DialogueDecisions
{
    public override bool GetDecision(LevelDialogueWindow entity)
    {
        return !entity.IsActiveAction;
    }
}
