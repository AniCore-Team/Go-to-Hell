using Sources;
using UnityEngine;
using Zenject;

public abstract class LocObject : MonoBehaviour
{
    [Inject] protected SettingsConfig settingsConfig;

    public abstract void OnSetLanguage();
}
