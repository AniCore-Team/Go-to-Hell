using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Button PlayGameButton;
        [Inject] private WindowsManager windowsManager;

        private void Awake()
        {
            windowsManager.Initialize();
            EventsTranslator.Call(WindowsTag.Hide);
            EventsTranslator.Call(WindowsTag.MenuButtons);
        }

        void Start()
        {
            Utils.AddListenerToButton(PlayGameButton,() => EventsTranslator.Call(WindowsTag.GameSlots));
        }

        private void OnDestroy()
        {
            EventsTranslator.RemoveAllListeners();
        }
    }
}

