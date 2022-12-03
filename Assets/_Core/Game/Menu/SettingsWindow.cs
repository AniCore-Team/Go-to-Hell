using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Menu
{
    public class SettingsWindow : UIWindow
    {
        [SerializeField] private TMP_Dropdown language;
        [SerializeField] private TMP_Dropdown quality;
        [SerializeField] private Scrollbar musicVolume;
        [SerializeField] private Scrollbar soundVolume;
        [SerializeField] private Button apply, reset;

        public void SetProperty()
        {
            ActivateWindow();
        }

        public void Apply()
        {
            EventsTranslator.Call(WindowsTag.Hide);
            EventsTranslator.Call(WindowsTag.MenuButtons);
        }

        public void Reset()
        {

        }
    }
}