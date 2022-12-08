using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Actions/MessageBattleDialogue", fileName = "MessageBattleDialogue", order = 52)]
public class MessageBattleDialogueAction : DialogueAction
{
    [SerializeField] private string message;
    [SerializeField] private bool isPlayerSpeak;

    public override void BeginAction(DialogueWindow entity)
    {
        base.BeginAction(entity);
        (entity as BattleDialogueWindow).PrintBattleMessage(message, isPlayerSpeak);
    }
}
