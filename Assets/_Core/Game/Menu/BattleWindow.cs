using Common;
using PureAnimator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BattleWindow : UIWindow
{
    [SerializeField] private AnimationCurve pulseCurve;
    [SerializeField] private AnimationCurve speedCurve;

    [SerializeField] private TMP_Text pointText;
    [SerializeField] private Button passButton;
    [SerializeField] private UICardSlot[] cardSlots;

    public event Action onPassRound;

    public void Init()
    {
        passButton.onClick.AddListener(() =>
            onPassRound?.Invoke());

        foreach (var slot in cardSlots)
            slot.Init();
    }

    public void SetPointText(int value)
    {
        pointText.text = value.ToString();
        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(1f, progress =>
            {
                pointText.transform.localScale = Vector3.one + Vector3.one * pulseCurve.Evaluate(progress) * 0.2f;
                return default;
            }, () => { });
    }

    public int GetCountFreeSlots()
    {
        int count = 0;
        foreach (var slot in cardSlots)
        {
            if (slot.card == null)
                count++;
        }
        return count;
    }

    public void SetCard(params CardView[] newCards)
    {
        //clear no loked card
        var freeSlots = GetFreeSlots();
        foreach (var newCard in newCards)
        {
            var freeSlot = freeSlots.Dequeue();
            freeSlot.card = newCard;
            freeSlot.card.transform.position = freeSlot.cardSpawn.position;
            freeSlot.IsLocked = false;
            newCard.Linked(freeSlot);
            Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(1f, progress =>
            {
                freeSlot.card.transform.position = Vector3.Lerp(
                    freeSlot.cardSpawn.position,
                    freeSlot.cardPoint.position,
                    progress * speedCurve.Evaluate(progress));
                return default;
            }, () => { });
        }
    }

    public void HideUnlockedCard(float progression)
    {
        Color origin = Color.white;
        Renderer renderer = null;
        foreach (var slot in cardSlots)
        {
            if (!slot.IsLocked && slot.card != null)
            {
                renderer = slot.card.transform.GetChild(0).GetComponent<MeshRenderer>();
                origin = renderer.material.color;
                renderer.material.color = new Color(origin.r, origin.g, origin.b, progression);
            }
        }
    }

    public void UnlockedAndClearCards()
    {
        foreach (var slot in cardSlots)
        {
            if (slot.IsLocked)
                slot.IsLocked = false;
            else if (slot.card != null)
            {
                slot.card.Destroy();
                slot.card = null;
            }
        }
    }

    private Queue<UICardSlot> GetFreeSlots()
    {
        Queue<UICardSlot> temp = new Queue<UICardSlot>();
        foreach (var slot in cardSlots)
        {
            if (slot.card == null)
                temp.Enqueue(slot);
        }
        return temp;
    }

    public void ShiftToFreeSlots()
    {
        for (int i = 1; i < cardSlots.Length; i++)
        {
            if (TryShiftCard(i, out var newSlot))
            {
                newSlot.card = cardSlots[i].card;
                cardSlots[i].card = null;
                var toSlot = newSlot;
                var fromSlot = cardSlots[i];
                Services<PureAnimatorController>
                .Get()
                .GetPureAnimator()
                .Play(1f, progress =>
                {
                    toSlot.card.transform.position = Vector3.Lerp(
                        fromSlot.cardPoint.position,
                        toSlot.cardPoint.position,
                        progress * speedCurve.Evaluate(progress));
                    return default;
                }, () => { });
            }
        }
    }

    private bool TryShiftCard(int slot, out UICardSlot position)
    {
        if (slot > 0 && cardSlots[slot - 1].card == null)
        {
            return TryShiftCard(slot - 1, out position);
        }

        position = cardSlots[slot];
        return true;
    }
}

[Serializable]
public class UICardSlot
{
    public CardView card;
    public Transform cardPoint;
    public Transform cardSpawn;
    [SerializeField] private Toggle locker;

    [SerializeField] private bool isLocked = false;

    public bool IsLocked
    {
        get => isLocked;
        set
        {
            isLocked = value;
            locker.isOn = value;
        }
    }

    public void Init()
    {
        locker.onValueChanged.AddListener(isOn => isLocked = isOn);
    }
}