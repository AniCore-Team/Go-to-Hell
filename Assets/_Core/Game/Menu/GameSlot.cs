using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    [RequireComponent(typeof(Button))]
    public class GameSlot : MonoBehaviour
    {
        [SerializeField] private GameObject newGamePanel;
        [SerializeField] private LoadSlotScreen loadGamePanel;
        [SerializeField] private Button clickableButton;

        public void SetProperty(GameSlotType type)
        {
            loadGamePanel.DisactivateWindow();
            newGamePanel.SetActive(false);

            switch (type)
            {
                case GameSlotType.New:
                    SetNew();
                    break;
                case GameSlotType.Full:
                    SetFull();
                    break;
            }
        }

        private void SetNew()
        {
            newGamePanel.SetActive(true);
            Utils.AddListenerToButton(clickableButton, () => EventsTranslator.Call(WindowsTag.Create));
        }

        private void SetFull()
        {
            loadGamePanel.ActivateWindow();
            loadGamePanel.SetProperty("client", "date", "level");
            Utils.AddListenerToButton(clickableButton, () => LoadingManager.OnLoadScene.Invoke("Location"));
        }
    }

    public enum GameSlotType
    {
        New, Full
    }
}