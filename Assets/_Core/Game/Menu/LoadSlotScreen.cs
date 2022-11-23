using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI.Menu
{
    public class LoadSlotScreen : UIWindow
    {
        [SerializeField] private TextMeshProUGUI clientNameText, lastSaveDateText, lastLevelText;
        [SerializeField] private Image screenshot;

        public void SetProperty(string clientName, string date, string level, Sprite sprite)
        {
            clientNameText.text = clientName;
            lastSaveDateText.text = date;
            lastLevelText.text = level;
            screenshot.sprite = sprite;
        }
    }
}