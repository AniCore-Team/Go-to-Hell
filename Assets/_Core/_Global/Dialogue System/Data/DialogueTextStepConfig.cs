using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "DialogueTextStepConfig", menuName = "ScriptableObjects/DialogueTextStepConfig", order = 1)]
    public class DialogueTextStepConfig : ScriptableObject
    {
        [SerializeField] private SystemLanguage currentLanguage;
        [SerializeField] private List<TextDialogueStep> steps;

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

        public string GetText(string nameStep) => string.IsNullOrEmpty(nameStep) ? "" :
            steps.FirstOrDefault(step => step.nameStep.Equals(nameStep))
            .texts.FirstOrDefault(pair => pair.langName == currentLanguage).text;
    }
}