using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Decisions/WaitAction", fileName = "WaitAction", order = 52)]
public class WaitActionDecision : DialogueDecisions
{
    public override bool GetDecision(DialogueWindow entity)
    {
        return !entity.IsActiveAction;
    }
}
