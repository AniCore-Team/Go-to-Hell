#pragma warning disable 649
using Config;
using System;
using UnityEngine;

namespace Sources
{
    [CreateAssetMenu(fileName = "SettingsConfig", menuName = "ScriptableObjects/SettingsConfig")]
    public class SettingsConfig : ScriptableObject
    {
        [SerializeField] private DialogueTextStepConfig textUIConfig;
        [SerializeField] private SystemLanguage currentLanguage = SystemLanguage.English;
        [SerializeField] private int currentQuality = 1;
        [SerializeField] private float currentMusic = 0.8f;
        [SerializeField] private float currentSound = 0.8f;

        public event Action onShiftLanguage;

        public SystemLanguage CurrentLanguage
        {
            get => currentLanguage;
            set
            { 
                currentLanguage = value;
                textUIConfig.SetLanguage(value);
                onShiftLanguage?.Invoke();
            }
        }
        public int CurrentQuality { get => currentQuality; set => currentQuality = value; }
        public float CurrentMusic { get => currentMusic; set => currentMusic = value; }
        public float CurrentSound { get => currentSound; set => currentSound = value; }

        public string GetUIText(string key)
        {
            return textUIConfig.GetText(key);
        }
    }
}