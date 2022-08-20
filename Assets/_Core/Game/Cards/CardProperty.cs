using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Create New Card")]
public class CardProperty : ScriptableObject
{
    public string name;
    public Sprite icon;
    public GameObject prefab;
    public int max_level;
}
