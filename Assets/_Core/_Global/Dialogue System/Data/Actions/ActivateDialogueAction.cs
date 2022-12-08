using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Dialogue/Actions/ActivateDialogue", fileName = "ActivateDialogue", order = 52)]
public class ActivateDialogueAction : DialogueAction
{
    [SerializeField] private bool isActivate;

    public override void BeginAction(DialogueWindow entity)
    {
        base.BeginAction(entity);
        entity.IsActiveAction = true;
        PureAnimation.Play(0.2f, progress =>
        {
            entity.SetVisionDialogue(isActivate ? progress : 1- progress);
            return default;
        }, () =>
        {
            entity.IsActiveAction = false;
            if (!isActivate)
                entity.CloseDialogue();
        });
    }
}
