using TMPro;
using UnityEngine;

public class LocalizationUITMPText : LocObject
{
    [SerializeField] private string key;

    private void OnEnable()
    {
        settingsConfig.onShiftLanguage += OnSetLanguage;
        OnSetLanguage();
    }

    private void OnDisable()
    {
        settingsConfig.onShiftLanguage -= OnSetLanguage;
    }

    public override void OnSetLanguage()
    {
        SetLanguage(GetComponent<TMP_Text>(), settingsConfig.GetUIText(key));
    }

    public TMP_Text SetLanguage(TMP_Text to, string key)
    {
        to.text = key;
        return to;
    }
}
