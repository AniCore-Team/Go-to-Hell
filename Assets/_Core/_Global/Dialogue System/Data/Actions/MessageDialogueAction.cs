using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Actions/MessageDialogue", fileName = "MessageDialogue", order = 52)]
public class MessageDialogueAction : DialogueAction
{
    [SerializeField] private string message;
    [SerializeField] private string answer1;
    [SerializeField] private string answer2;
    [SerializeField] private string answer3;

    public override void BeginAction(LevelDialogueWindow entity)
    {
        base.BeginAction(entity);
        entity.PrintMessages(message, answer1, answer2, answer3);
    }
}
