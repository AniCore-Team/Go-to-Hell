using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UICardSlot
{
    public CardView card;
    public Transform cardPoint;
    public Transform cardSpawn;
    [SerializeField] private Button locker;
    [SerializeField] private GameObject checkMark;

    [SerializeField] private bool isLocked = false;

    private event Func<bool, bool> onUseLock;
    private Action onCheclLockers;

    public bool IsLocked
    {
        get => isLocked;
    }

    public void Init(Func<bool, bool> func, Action onCheclLockers)
    {
        onUseLock = func;
        this.onCheclLockers = onCheclLockers;
        locker.onClick.AddListener(() =>
        {
            SetLocked(!isLocked, true);
        });
    }

    public void SetLocked(bool value, bool isUseScore)
    {
        if (isUseScore)
            if (!onUseLock(!isLocked))
                return;

        isLocked = value;
        onCheclLockers?.Invoke();
        checkMark.SetActive(isLocked);
    }

    public void SetActiveLocker(bool isActive)
    {
        locker.gameObject.SetActive(isActive);
    }

    public void SetInteractableLocker(bool isActive)
    {
        locker.interactable = isActive;
    }
}