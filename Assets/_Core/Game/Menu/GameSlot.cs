using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    [RequireComponent(typeof(Button))]
    public class GameSlot : MonoBehaviour
    {
        [SerializeField] private GameObject newGamePanel;
        [SerializeField] private LoadSlotScreen loadGamePanel;
        [SerializeField] private Button clickableButton;

        [Inject] private SaveManager saveManager;
        [Inject] private CardsList cardsList;
        [Inject] private Client client;

        private SaveSlotData saveSlotData;

        public void SetProperty(GameSlotType type, SaveSlotData data = default)
        {
            saveSlotData = data;
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
            Utils.AddListenerToButton(clickableButton, () =>
            {
                saveManager.CurrentSlot = saveSlotData.id;
                EventsTranslator.Call(WindowsTag.Create);
            });
        }

        private void SetFull()
        {
            loadGamePanel.ActivateWindow();
            loadGamePanel.SetProperty(
                saveManager.GetSlotData(saveSlotData.id).clientModel.nameClient,
                saveManager.GetSlotData(saveSlotData.id).clientModel.date.ToString(),
                saveManager.GetSlotData(saveSlotData.id).levelModel.circle.ToString(),
                saveManager.GetSlotData(saveSlotData.id).sprite);
            Utils.AddListenerToButton(clickableButton, () =>
            {
                saveManager.CurrentSlot = saveSlotData.id;
                client.SetClientModel(saveManager.GetSlotData().clientModel, cardsList);
                LoadingManager.OnLoadScene.Invoke("Location");
            });
        }
    }

    public enum GameSlotType
    {
        New, Full
    }
}