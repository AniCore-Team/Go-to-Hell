using Common;
using Config;
using PureAnimator;
using Sources;
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

public class DialogueWindow : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Text dialogueText;
    [SerializeField] protected Text nameText;
    [SerializeField] private DialogueTextStepConfig names;

    [Inject] protected AudioManager audioManager;
    [Inject] private SettingsConfig settingsConfig;

    protected Answers answer;
    protected DialogueBehaviourController controller;
    protected DialogueTextStepConfig dialogueTextStepConfig;
    protected DialogueAudioStepConfig dialogueAudioStepConfig;

    public bool IsActiveAction { get; set; }

    protected void Awake()
    {
        canvasGroup.alpha = 0;
    }

    protected void Update()
    {
        controller?.BehaviourUpdate();
    }

    protected void LateUpdate()
    {
        controller?.LateBehaviourUpdate();
    }

    public void SetVisionDialogue(float value)
    {
        canvasGroup.alpha = value;
    }

    public virtual void StartDialouge(DialogueData dialogueData)
    {
        dialogueTextStepConfig = dialogueData.textSteps;
        dialogueTextStepConfig.SetLanguage(settingsConfig.CurrentLanguage);
        dialogueAudioStepConfig = dialogueData.audioSteps;
        nameText.text = names.GetText(dialogueData.name);
        controller = new DialogueBehaviourController();
        controller.TryInstall(this, dialogueData.graph);
    }

    public virtual void CloseDialogue()
    {
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

    public virtual void PrintMessages(string messageKey, params string[] answerKeys)
    {
        answer = Answers.None;
        dialogueText.text = dialogueTextStepConfig.GetText(messageKey);
        audioManager.PlayVoice(dialogueAudioStepConfig.GetAudio(messageKey));
    }
}
