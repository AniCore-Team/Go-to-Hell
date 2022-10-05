using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetEffect
{
    All,
    Self,
    Other
}
public enum CardID
{
    FireBall,
    ArchangelFeather
}

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Create New Card")]
public class CardProperty : ScriptableObject
{
    public string name;
    public CardID id;
    public Sprite icon;
    public GameObject prefab;
    public int max_level;
    public int cost;
    public int duration;
    public TargetEffect target;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (prefab != null)
            name = prefab.name;
    }
#endif
}
