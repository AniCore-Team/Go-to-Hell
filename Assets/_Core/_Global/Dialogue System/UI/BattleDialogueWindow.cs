using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BattleDialogueWindow : DialogueWindow
{
    [SerializeField] private Image portrait;
    [SerializeField] private Button answerButtons;

    [Inject] private CinemachineSwitcher cinemachineSwitcher;

    public event Action endDialog;

    public void StartBattleDialog(DialogueData dialogueData, bool isPlayer)
    {
        cinemachineSwitcher.SwitchState(isPlayer ?
            CinemachineSwitcher.CinemachineState.PlayerDialog :
            CinemachineSwitcher.CinemachineState.EnemyDialog);

        StartDialouge(dialogueData);
    }

    public override void StartDialouge(DialogueData dialogueData)
    {
        base.StartDialouge(dialogueData);
        portrait.sprite = dialogueData.portrait;
        answerButtons.onClick.RemoveAllListeners();
        answerButtons.onClick.AddListener(() => answer = Answers.First);
    }

    public override void CloseDialogue()
    {
        base.CloseDialogue();
        endDialog?.Invoke();
        endDialog = null;
    }

    public void PrintBattleMessage(Sprite portrait, string name, string messageKey, bool isPlayerSpeak)
    {
        cinemachineSwitcher.SwitchState(isPlayerSpeak ?
            CinemachineSwitcher.CinemachineState.PlayerDialog :
            CinemachineSwitcher.CinemachineState.EnemyDialog);

        this.portrait.sprite = portrait;
        nameText.text = name;
        PrintMessages(messageKey);
    }
}
