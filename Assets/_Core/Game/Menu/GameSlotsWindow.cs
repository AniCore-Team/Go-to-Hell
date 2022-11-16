using System.Collections;
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

        private List<GameSlot> list = new List<GameSlot>();

        public void SetProperty()
        {
            ClearSlots();

            EventsTranslator.Call(WindowsTag.Hide);

            Utils.AddListenerToButton(closeWindowButton, Close);

            for (int i = 0; i < slotsCount; i++)
            {
                CreateNewSlot(GameSlotType.New);
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

        private void CreateNewSlot(GameSlotType type)
        {
            var slot = Instantiate(slotPrefab);
            slot.SetProperty(type);
            Utils.SetParentToTransform(slot.transform, container);

            list.Add(slot);
        }
    }
}