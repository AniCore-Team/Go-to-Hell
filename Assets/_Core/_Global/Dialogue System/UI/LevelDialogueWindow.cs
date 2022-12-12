using Config;
using Sources;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelDialogueWindow : DialogueWindow
{
    [SerializeField] private Image portrait;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Text[] answerTexts;

    private LocationPlayerController playerController;

    private LocationPlayerController PlayerController
    {
        get
        {
            if (playerController == null)
                playerController = FindObjectOfType<LocationPlayerController>();
            return playerController;
        }
    }

    public override void StartDialouge(DialogueData dialogueData)
    {
        base.StartDialouge(dialogueData);
        PlayerController.enabled = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            var button = answerButtons[i];
            var ans = (Answers)i;
            button.interactable = false;
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                answer = ans;
            });
        }
    }

    public override void CloseDialogue()
    {
        base.CloseDialogue();
        PlayerController.enabled = true;
    }

    public override void PrintMessages(string messageKey, params string[] answerKeys)
    {
        base.PrintMessages(messageKey, answerKeys);
        foreach (var button in answerButtons)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < answerKeys.Length; i++)
        {
            if (string.IsNullOrEmpty(answerKeys[i]))
                continue;
            answerTexts[i].text = dialogueTextStepConfig.GetText(answerKeys[i]);
            answerButtons[i].interactable = true;
            answerButtons[i].gameObject.SetActive(true);
        }
    }
}
