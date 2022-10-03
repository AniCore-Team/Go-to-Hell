using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Property", menuName = "Enemies/Create New Enemy Property")]
public class EnemyProperty : ScriptableObject
{
    public GameObject prefab;
    public List<CardProperty> simpleCards;
    public List<HardAttack> hardAttacks;
}
