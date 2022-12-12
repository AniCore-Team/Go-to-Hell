using Common;
using PureAnimator;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Button locker;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private GameObject outline;

    private DeckSlot property;
    [HideInInspector] public UICardSlot linkSlot;

    private bool isHightLight = false;

    public Button LockerButton => locker;
    public GameObject CheckMark => checkMark;
    //public GameObject Outline => outline;

    public DeckSlot Property
    {
        get => property;
        set
        {
            property = value;
            priceText.text = property.card.cost.ToString();
        }
    }


    private void Start()
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    public void Linked(UICardSlot linkSlot)
    {
        this.linkSlot = linkSlot;
        this.linkSlot.SetActiveLocker(true);
        transform.SetParent(linkSlot.cardPoint);
        transform.localScale = Vector3.one * 600f;
        locker.onClick.RemoveAllListeners();
        locker.onClick.AddListener(() => linkSlot.onSetLockers?.Invoke());
    }

    public void Use(Action endCommand)
    {
        linkSlot.SetLocked(false, false);
        linkSlot.SetActiveLocker(false);
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
        Destroy(gameObject);
    }

    public void HightLight(bool isActive)
    {
        if (isHightLight == isActive) return;
        isHightLight = isActive;

        outline.SetActive(isActive);

        //Services<PureAnimatorController>
        //    .Get()
        //    .GetPureAnimator()
        //    .Play(0.2f, progress =>
        //    {
        //        if (this == null) return default;
        //        transform.localScale = (Vector3.one + Vector3.one * 0.2f * (isActive ? progress : 1 - progress)) * 600f;
        //        return default;
        //    }, () => { });
    }

    public void Drop()
    {
        Destroy();
    }

    public void SetLocker(Action action)
    {

    }
}
