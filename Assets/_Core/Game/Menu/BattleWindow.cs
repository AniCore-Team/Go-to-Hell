using Common;
using PureAnimator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleWindow : UIWindow
{
    [SerializeField] private CanvasGroup BottomPanel;
    [SerializeField] private CanvasGroup TopPanel;
    [SerializeField] private CanvasGroup WinPanel;

    [SerializeField] private Button doneButton;
    [SerializeField] private Image newCard;


    [SerializeField] private AnimationCurve pulseCurve;
    [SerializeField] private AnimationCurve speedCurve;

    [SerializeField] private TMP_Text pointText;
    [SerializeField] private Button passButton;
    [SerializeField] private CharacterHUD playerHUD;
    [SerializeField] private CharacterHUD enemyHUD;
    [SerializeField] private UICardSlot[] cardSlots;

    private RectTransform downPanel;
    private bool isShowDownPanel;
    private int battlePoint;

    public event Action onPassRound;

    public CharacterHUD PlayerHUD => playerHUD;
    public CharacterHUD EnemyHUD => enemyHUD;

    public void Init(Func<bool, bool> func)
    {
        BottomPanel.alpha = 1;
        TopPanel.alpha = 1;
        WinPanel.alpha = 0;

        downPanel = transform.GetChild(0).GetComponent<RectTransform>();
        passButton.onClick.AddListener(() =>
            onPassRound?.Invoke());

        playerHUD.Init();
        enemyHUD.Init();

        foreach (var slot in cardSlots)
            slot.Init(func, CheckLockers);
    }

    public void ShowWin(Sprite newCard, Action winFunc)
    {
        BottomPanel.gameObject.SetActive(false);
        TopPanel.alpha = 0;
        WinPanel.alpha = 1;

        this.newCard.sprite = newCard;
        doneButton.onClick.AddListener(() => winFunc());
    }

    private void CheckLockers()
    {
        if (battlePoint == 0)
        {
            foreach (var card in cardSlots)
                if (!card.IsLocked)
                    card.SetInteractableLocker(false);
        }
        else
            foreach (var card in cardSlots)
                card.SetInteractableLocker(true);
    }

    public void RepaintPointText(int value)
    {
        battlePoint = value;
        pointText.text = battlePoint.ToString();

        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(1f, progress =>
            {
                pointText.transform.localScale = Vector3.one + Vector3.one * pulseCurve.Evaluate(progress) * 0.2f;
                return default;
            }, () => { });
    }

    public void SetActiveBottomPanel(bool value)
    {
        downPanel.GetComponent<CanvasGroup>().interactable = value;
        downPanel.GetComponent<CanvasGroup>().blocksRaycasts = value;
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

    public void SetCard(Action endCommand, params CardView[] newCards)
    {
        //clear no loked card
        var freeSlots = GetFreeSlots();
        for (var i = 0; i < newCards.Length; i++)
        {
            var innerI = i;
            var newCard = newCards[i];
            var freeSlot = freeSlots.Dequeue();
            freeSlot.card = newCard;
            freeSlot.card.transform.position = freeSlot.cardSpawn.position;
            freeSlot.SetLocked(false, false);
            newCard.Linked(freeSlot);
            newCard.transform.localRotation = Quaternion.Euler(0, 180, 0);
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
            }, () =>
            {
                if (innerI > 0) return;
                SetActiveBottomPanel(true);
                endCommand?.Invoke();
            });
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
                slot.SetLocked(false, false);
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
            if (cardSlots[i].card == null) continue;
            if (TryShiftCard(i, out var newSlot))
            {
                var toSlot = newSlot;
                var fromSlot = cardSlots[i];

                toSlot.card = fromSlot.card;
                toSlot.card.Linked(toSlot);
                fromSlot.card = null;
                fromSlot.SetActiveLocker(false);
                toSlot.SetLocked(fromSlot.IsLocked, false);
                fromSlot.SetLocked(false, false);

                Services<PureAnimatorController>
                .Get()
                .GetPureAnimator()
                .Play(0.3f, progress =>
                {
                    toSlot.card.transform.position = Vector3.Lerp(
                        fromSlot.cardPoint.position,
                        toSlot.cardPoint.position,
                        progress * speedCurve.Evaluate(progress));

                    toSlot.card.transform.rotation = Quaternion.Euler(
                        toSlot.card.transform.eulerAngles.x,
                        toSlot.card.transform.eulerAngles.y,
                        progress * pulseCurve.Evaluate(progress) * 10f);
                    return default;
                }, () => { });
            }
        }
    }

    private bool TryShiftCard(int slot, out UICardSlot position, bool prevShift = false)
    {
        position = cardSlots[slot];
        if (slot == 0 || cardSlots[slot - 1].card != null)
            return prevShift;

        prevShift = true;
        return TryShiftCard(slot - 1, out position, prevShift);
    }

    public void ShowPanel(Action endMove = default)
    {
        if (isShowDownPanel)
        {
            endMove?.Invoke();
            return;
        }

        Services<PureAnimatorController>
        .Get()
        .GetPureAnimator()
        .Play(0.3f, progress =>
        {
            downPanel.anchoredPosition = new Vector2(downPanel.anchoredPosition.x,
                Mathf.Lerp(-downPanel.rect.height, 0, progress));
            return default;
        }, () => 
        {
            isShowDownPanel = true;
            endMove?.Invoke();
        });
    }

    public void HidePanel(Action endMove = default)
    {
        if (!isShowDownPanel)
        {
            endMove?.Invoke();
            return;
        }

        Services<PureAnimatorController>
        .Get()
        .GetPureAnimator()
        .Play(0.3f, progress =>
        {
            downPanel.anchoredPosition = new Vector2(downPanel.anchoredPosition.x,
                Mathf.Lerp(0, -downPanel.rect.height, progress));
            return default;
        }, () =>
        {
            isShowDownPanel = false;
            endMove?.Invoke();
        });
    }
}
