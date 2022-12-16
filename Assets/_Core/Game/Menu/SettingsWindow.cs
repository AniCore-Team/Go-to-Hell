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
            string[] names = QualitySettings.names;
            List<TMP_Dropdown.OptionData> options = new();
            foreach (string key in names)
                options.Add(new TMP_Dropdown.OptionData(key));
            quality.options = options;
            quality.onValueChanged.RemoveAllListeners();
            quality.onValueChanged.AddListener(i =>
            {
                bufferQuality = i;
                //settingsConfig.CurrentQuality = i;
                //QualitySettings.SetQualityLevel(i, true);
            });
            QualitySettings.SetQualityLevel(settingsConfig.CurrentQuality, true);

            options = new();
            foreach (var key in languages)
                options.Add(new TMP_Dropdown.OptionData(key.ToString()));
            language.options = options;
            language.onValueChanged.RemoveAllListeners();
            language.onValueChanged.AddListener(i =>
            {
                bufferLanguage = languages[i];
                //settingsConfig.CurrentLanguage = languages[i];
            });

            musicVolume.value = settingsConfig.CurrentMusic;
            soundVolume.value = settingsConfig.CurrentSound;
            musicVolume.onValueChanged.RemoveAllListeners();
            musicVolume.onValueChanged.AddListener(i =>
            {
                bufferMusic = i;
                //settingsConfig.CurrentMusic = i;
                //audioManager.SetMusicVolume((settingsConfig.CurrentMusic * 80f) - 80f);
            });
            soundVolume.onValueChanged.RemoveAllListeners();
            soundVolume.onValueChanged.AddListener(i =>
            {
                bufferSound = i;
                //settingsConfig.CurrentSound = i;
                //audioManager.SetSoundVolume((settingsConfig.CurrentSound * 80f) - 80f);
                //audioManager.SetVoiceVolume((settingsConfig.CurrentSound * 80f) - 80f);
            });
            audioManager.SetMusicVolume((settingsConfig.CurrentMusic * 80f) - 80f);
            audioManager.SetSoundVolume((settingsConfig.CurrentSound * 80f) - 80f);
            audioManager.SetVoiceVolume((settingsConfig.CurrentSound * 80f) - 80f);
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