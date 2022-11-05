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
    IcePrison,
    ArchangelFeather,
    BloodRain,
    EdemApple,
    Shield,
    EdemAngel,
    HellDog,
    JacksShadow,
    Stun,
    HellSwarm,
    HellScream
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
    public BaseActions effectAction;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (prefab != null)
            name = prefab.name;
    }
#endif
}
