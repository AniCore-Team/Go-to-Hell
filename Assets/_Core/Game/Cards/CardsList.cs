using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cards List", menuName = "Cards/Cards List")]
public class CardsList : ScriptableObject
{
    public List<CardProperty> list;
}
