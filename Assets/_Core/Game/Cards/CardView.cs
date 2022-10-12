using Common;
using PureAnimator;
using System;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private Rigidbody rigidbody;

    private DeckSlot property;
    [HideInInspector] public UICardSlot linkSlot;

    private bool isHightLight = false;

    public DeckSlot Property
    {
        get => property;
        set
        {
            property = value;
            priceText.text = property.card.cost.ToString();
        }
    }

    public void Linked(UICardSlot linkSlot)
    {
        this.linkSlot = linkSlot;
        transform.SetParent(linkSlot.cardPoint);
        transform.localScale = Vector3.one * 600f;
    }

    public void Use(Action endCommand)
    {
        linkSlot.SetLocked(false, false);
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
                transform.localScale = (Vector3.one + Vector3.one * 0.2f * (isActive ? progress : 1 - progress)) * 600f;
                return default;
            }, () => { });
    }

    public void Drop()
    {
        Destroy();
    }
}
