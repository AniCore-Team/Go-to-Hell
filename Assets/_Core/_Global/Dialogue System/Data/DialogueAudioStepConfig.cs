using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "DialogueAudioStepConfig", menuName = "ScriptableObjects/DialogueAudioStepConfig", order = 1)]
    public class DialogueAudioStepConfig : ScriptableObject
    {
        [SerializeField] private SystemLanguage currentLanguage;
        [SerializeField] private List<AudioDialogueStep> steps;

        public SystemLanguage CurrentLanguage => currentLanguage;

        public void SetLanguage(SystemLanguage language) => currentLanguage = language;

        public void CheckLanguage()
        {
            currentLanguage = Application.systemLanguage switch
            {
                SystemLanguage.Russian => SystemLanguage.Russian,
                _ => SystemLanguage.English
            };
        }

        public AudioClip GetAudio(string nameStep) => //string.IsNullOrEmpty(nameStep) ? "" :
            steps.FirstOrDefault(step => step.nameStep.Equals(nameStep))
            .audios.FirstOrDefault(pair => pair.langName == currentLanguage).audio;
    }
}