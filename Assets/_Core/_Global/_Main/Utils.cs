using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using PureAnimator;

public class Utils
{
    public static void SetParentToTransform(Transform current, Transform parent)
    {
        current.SetParent(parent);
        current.localScale = Vector3.one;
    }

    public static void AddListenerToButton(Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action());
    }

    public static TransformChanges EmptyPureAnimation(float progress) => default;
}