using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Sources;

namespace UI.Menu
{
    public class SettingsWindow : UIWindow
    {
        [SerializeField] private TMP_Dropdown language;
        [SerializeField] private TMP_Dropdown quality;
        [SerializeField] private Scrollbar musicVolume;
        [SerializeField] private Scrollbar soundVolume;
        [SerializeField] private Button apply, reset;
        [SerializeField] private List<SystemLanguage> languages;

        [Inject] private AudioManager audioManager;
        [Inject] private SettingsConfig settingsConfig;

        private SystemLanguage bufferLanguage;
        private int bufferQuality;
        private float bufferMusic;
        private float bufferSound;

        public void Init()
        {
            quality.onValueChanged.RemoveAllListeners();
            quality.onValueChanged.AddListener(i =>
            {
                bufferQuality = i;
            });
            QualitySettings.SetQualityLevel(settingsConfig.CurrentQuality, true);

            language.onValueChanged.RemoveAllListeners();
            language.onValueChanged.AddListener(i =>
            {
                bufferLanguage = languages[i];
            });

            musicVolume.value = settingsConfig.CurrentMusic;
            soundVolume.value = settingsConfig.CurrentSound;
            musicVolume.onValueChanged.RemoveAllListeners();
            musicVolume.onValueChanged.AddListener(i =>
            {
                bufferMusic = i;
            });
            soundVolume.onValueChanged.RemoveAllListeners();
            soundVolume.onValueChanged.AddListener(i =>
            {
                bufferSound = i;
            });
            audioManager.SetMusicVolume((settingsConfig.CurrentMusic * 80f) - 80f);
            audioManager.SetSoundVolume((settingsConfig.CurrentSound * 80f) - 80f);
            audioManager.SetVoiceVolume((settingsConfig.CurrentSound * 80f) - 80f);
        }

        private void OnEnable()
        {
            bufferQuality = settingsConfig.CurrentQuality;
            bufferLanguage = settingsConfig.CurrentLanguage;
            bufferMusic = settingsConfig.CurrentMusic;
            bufferSound = settingsConfig.CurrentSound;
        }

        public void SetProperty()
        {
            ActivateWindow();
        }

        public void Apply()
        {
            EventsTranslator.Call(WindowsTag.Hide);
            EventsTranslator.Call(WindowsTag.MenuButtons);

            settingsConfig.CurrentQuality = bufferQuality;
            QualitySettings.SetQualityLevel(settingsConfig.CurrentQuality, true);
            settingsConfig.CurrentLanguage = bufferLanguage;
            settingsConfig.CurrentMusic = bufferMusic;
            audioManager.SetMusicVolume((settingsConfig.CurrentMusic * 80f) - 80f);
            settingsConfig.CurrentSound = bufferSound;
            audioManager.SetSoundVolume((settingsConfig.CurrentSound * 80f) - 80f);
            audioManager.SetVoiceVolume((settingsConfig.CurrentSound * 80f) - 80f);
        }

        public void ResetValue()
        {
            bufferQuality = quality.value = 1;
            bufferLanguage = SystemLanguage.English;
            language.value = languages.IndexOf(SystemLanguage.English);
            bufferMusic = musicVolume.value = 0.8f;
            bufferSound = soundVolume.value = 0.8f;

            settingsConfig.CurrentQuality = bufferQuality;
            QualitySettings.SetQualityLevel(settingsConfig.CurrentQuality, true);
            settingsConfig.CurrentLanguage = bufferLanguage;
            settingsConfig.CurrentMusic = bufferMusic;
            audioManager.SetMusicVolume((settingsConfig.CurrentMusic * 80f) - 80f);
            settingsConfig.CurrentSound = bufferSound;
            audioManager.SetSoundVolume((settingsConfig.CurrentSound * 80f) - 80f);
            audioManager.SetVoiceVolume((settingsConfig.CurrentSound * 80f) - 80f);
        }
    }
}