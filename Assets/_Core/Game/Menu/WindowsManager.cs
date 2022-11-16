using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI.Menu
{
    public class WindowsManager
    {
        private GameSlotsWindow gameSlotsWindow;
        private CreateNewGameWindow createNewGameWindow;
        private MenuButtonsWindow menuButtonsWindow;

        [Inject]
        public void Construct(GameSlotsWindow gameSlotsWindow, CreateNewGameWindow createNewGameWindow, MenuButtonsWindow menuButtonsWindow)
        {
            this.gameSlotsWindow = gameSlotsWindow;
            this.createNewGameWindow = createNewGameWindow;
            this.menuButtonsWindow = menuButtonsWindow;
        }

        public void Initialize()
        {
            EventsTranslator.AddListener(WindowsTag.Create, createNewGameWindow.SetProperty);
            EventsTranslator.AddListener(WindowsTag.GameSlots, gameSlotsWindow.SetProperty);
            EventsTranslator.AddListener(WindowsTag.MenuButtons, menuButtonsWindow.ActivateWindow);
            EventsTranslator.AddListener(WindowsTag.Hide, HideAllWindows);
        }

        private void HideAllWindows()
        {
            gameSlotsWindow.DisactivateWindow();
            createNewGameWindow.DisactivateWindow();
            menuButtonsWindow.DisactivateWindow();
        }
    }

    public static class WindowsTag
    {
        public const string Create = "Create";
        public const string GameSlots = "GameSlots";
        public const string MenuButtons = "MenuButtons";
        public const string Hide = "Hide";
    }
}