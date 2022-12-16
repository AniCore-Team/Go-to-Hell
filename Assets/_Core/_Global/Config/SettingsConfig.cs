#pragma warning disable 649
using UnityEngine;

namespace Sources
{
    [CreateAssetMenu(fileName = "SettingsConfig", menuName = "ScriptableObjects/SettingsConfig")]
    public class SettingsConfig : ScriptableObject
    {
        [SerializeField] private SystemLanguage currentLanguage = SystemLanguage.English;
        [SerializeField] private int currentQuality = 1;
        [SerializeField] private float currentMusic = 0.8f;
        [SerializeField] private float currentSound = 0.8f;

        public SystemLanguage CurrentLanguage
        {
            get => currentLanguage;
            set
            { 
                currentLanguage = value;
                //onShiftLanguage?.Invoke();
            }
        }
        public int CurrentQuality { get => currentQuality; set => currentQuality = value; }
        public float CurrentMusic { get => currentMusic; set => currentMusic = value; }
        public float CurrentSound { get => currentSound; set => currentSound = value; }
    }
}