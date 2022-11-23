using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class GameSlotsWindow : UIWindow
    {
        [SerializeField] private GameSlot slotPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private Button closeWindowButton;
        [SerializeField] private int slotsCount;

        [Inject] private SaveManager saveManager;
        [Inject] private Factory<GameSlot> factory;

        private List<GameSlot> list = new List<GameSlot>();

        public void SetProperty()
        {
            ClearSlots();

            EventsTranslator.Call(WindowsTag.Hide);

            Utils.AddListenerToButton(closeWindowButton, Close);

            saveManager.Load();

            for (int i = 0; i < slotsCount; i++)
            {
                if (saveManager.HasSlot(i))
                    CreateNewSlot(GameSlotType.Full, saveManager.GetSlotData(i));
                else
                    CreateNewSlot(GameSlotType.New, new SaveSlotData { id = i });
            }

            ActivateWindow();
        }

        private void Close()
        {
            EventsTranslator.Call(WindowsTag.Hide);
            EventsTranslator.Call(WindowsTag.MenuButtons);
        }

        private void ClearSlots()
        {
            foreach (var item in list)
            {
                Destroy(item.gameObject);
            }

            list.Clear();
        }

        private void CreateNewSlot(GameSlotType type, SaveSlotData data = default)
        {
            var slot = factory.Create(slotPrefab.gameObject);
            slot.SetProperty(type, data);
            Utils.SetParentToTransform(slot.transform, container);

            list.Add(slot);
        }
    }
}