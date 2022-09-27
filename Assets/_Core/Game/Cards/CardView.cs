using Common;
using PureAnimator;
using System;
using UnityEngine;

public class CardView : MonoBehaviour
{
    public DeckSlot property;
    public UICardSlot linkSlot;

    private bool isHightLight = false;

    public void Linked(UICardSlot linkSlot)
    {
        this.linkSlot = linkSlot;
    }

    public void Use(Action endCommand)
    {
        linkSlot.IsLocked = false;
        linkSlot.card = null;
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(0.5f, progress =>
            {
                Color origin = Color.white;
                Renderer renderer = null;

                renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
                origin = renderer.material.color;
                renderer.material.color = new Color(origin.r, origin.g, origin.b, 1 - progress);

                return default;
            }, () =>
            { Destroy(); endCommand?.Invoke(); });
    }

    public void Destroy()
    {
        UnityEngine.Object.Destroy(gameObject);
    }

    public void HightLight(bool isActive)
    {
        if (isHightLight == isActive) return;
        isHightLight = isActive;

        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(0.2f, progress =>
            {
                if (this == null) return default;
                transform.localScale = Vector3.one + Vector3.one * 0.2f * (isActive ? progress : 1 - progress) ;
                return default;
            }, () => { });
    }
}
