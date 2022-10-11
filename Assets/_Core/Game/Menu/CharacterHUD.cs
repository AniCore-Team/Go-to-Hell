using Common;
using PureAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHUD : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private PureAnimation PureAnimation => Services<PureAnimatorController>.Get().GetPureAnimator();

    public void Init()
    {
        fillImage.fillAmount = 1f;
    }

    public void SetHealth(float value)
    {
        var oldValue = fillImage.fillAmount;
        PureAnimation.Play(0.5f,
            progress =>
            {
                fillImage.fillAmount = Mathf.Lerp(oldValue, value, progress);
                return default;
            }, () => { });
    }
}
