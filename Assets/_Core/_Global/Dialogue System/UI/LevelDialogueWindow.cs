using Common;
using Config;
using PureAnimator;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public struct DialogueData
{
    public string name;
    public Sprite portrait;
    public DialogueTextStepConfig textSteps;
    public DialogueAudioStepConfig audioSteps;
    public DialogueBehaviourGraph graph;
}

public enum Answers
{
    None = -1,
    First,
    Second,
    Third
}

public class LevelDialogueWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image portrait;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Text[] answerTexts;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text nameText;

    [Inject] private SceneSpawner sceneSpawner;

    private Answers answer;
    private DialogueBehaviourController controller;
    private DialogueTextStepConfig dialogueTextStepConfig;
    private DialogueAudioStepConfig dialogueAudioStepConfig;

    public bool IsActiveAction { get; set; }
    public SceneSpawner SceneSpawner => sceneSpawner;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }

    private void Update()
    {
        controller?.BehaviourUpdate();
    }

    private void LateUpdate()
    {
        controller?.LateBehaviourUpdate();
    }

    public void SetVisionDialogue(float value)
    {
        canvasGroup.alpha = value;
    }

    public void StartDialouge(DialogueData dialogueData)
    {
        dialogueTextStepConfig = dialogueData.textSteps;
        dialogueAudioStepConfig = dialogueData.audioSteps;
        portrait.sprite = dialogueData.portrait;
        nameText.text = dialogueData.name;
        controller = new DialogueBehaviourController();
        controller.TryInstall(this, dialogueData.graph);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            var button = answerButtons[i];
            var ans = (Answers)i;
            button.interactable = false;
            button.gameObject.SetActive(false);
            button.onClick.AddListener(() =>
            {
                answer = ans;
            });
        }
    }

    public void CloseDialogue()
    {
        controller.TryUninstall();
        canvasGroup.alpha = 0;
    }

    public bool CheckAnswers(Answers answer)
    {
        if (this.answer == answer)
        {
            this.answer = Answers.None;
            return true;
        }
        return false;
    }

    public void PrintMessages(string messageKey, params string[] answerKeys)
    {
        answer = Answers.None;
        foreach (var button in answerButtons)
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }

        dialogueText.text = dialogueTextStepConfig.GetText(messageKey);
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
