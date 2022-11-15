using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI.Menu
{
    public class LoadSlotScreen : UIWindow
    {
        [SerializeField] private TextMeshProUGUI clientNameText, lastSaveDateText, lastLevelText;

        public void SetProperty(string clientName, string date, string level)
        {
            clientNameText.text = clientName;
            lastSaveDateText.text = date;
            lastLevelText.text = level;
        }
    }
}