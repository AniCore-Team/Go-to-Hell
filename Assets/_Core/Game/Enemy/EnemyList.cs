using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy List", menuName = "Enemies/Enemy List")]
public class EnemyList : ScriptableObject
{
    public List<EnemyProperty> list;
}
