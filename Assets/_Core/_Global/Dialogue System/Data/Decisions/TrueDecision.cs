using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Decisions/TrueDecision", fileName = "TrueDecision", order = 52)]
public class TrueDecision : DialogueDecisions
{
    public override bool GetDecision(LevelDialogueWindow entity)
    {
        return true;
    }
}
