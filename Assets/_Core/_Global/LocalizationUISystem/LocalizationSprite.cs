//using PixelCrushers;
//using UnityEngine;
//using UnityEngine.UI;

//public class LocalizationSprite : LocObject
//{
//    [SerializeField] private StringField t_UIElement;

//    public StringField TUIElement
//    {
//        get
//        {
//            return t_UIElement;
//        }

//        set
//        {
//            t_UIElement = value;
//        }
//    }

//    private void OnEnable()
//    {
//        LanguageSettings.language += OnSetLanguage;
//        OnSetLanguage();
//    }

//    private void OnDisable()
//    {
//        LanguageSettings.language -= OnSetLanguage;
//    }

//    public override void OnSetLanguage()
//    {
//        SetLanguage(this.GetComponent<Image>(), t_UIElement.value);
//    }

//    public static Image SetLanguage(Image to, string key)
//    {
//        Sprite sprite = Resources.Load<Sprite>(key);
//        to.sprite = sprite;
//        return to;
//    }
//}
