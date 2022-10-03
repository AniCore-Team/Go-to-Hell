using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HardAttack
{
    public int interval;
    [Range(0, 1)]public float chance;
    public List<CardProperty> hardCards;
}
