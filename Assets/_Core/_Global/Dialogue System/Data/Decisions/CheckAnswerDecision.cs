using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Decisions/CheckAnswer", fileName = "CheckAnswer", order = 52)]
public class CheckAnswerDecision : DialogueDecisions
{
    [SerializeField] private Answers answer;
    public override bool GetDecision(LevelDialogueWindow entity)
    {
        return entity.CheckAnswers(answer);
    }
}
