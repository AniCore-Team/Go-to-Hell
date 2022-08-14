using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    #region HP
    private float hp;
    private float maxHP;
    public float Hp => hp;
    public float MaxHP => maxHP;

    public void AddHP(float val)
    {
        hp += val;

        if(hp > maxHP)
        {
            hp = maxHP;
        }
    }

    public void RemoveHP(float val)
    {
        hp -= val;

        if(hp <= 0)
        {
            hp = 0;
        }
    }
    #endregion

    #region SHIELD
    private int shield;
    public int Shield => shield;

    public void AddShield(int val)
    {
        shield += val;
    }

    public void RemoveShield(int val)
    {
        shield -= val;

        if(shield < 0)
        {
            shield = 0;
        }
    }
    #endregion

    #region LIFE
    private int liveCount;
    private const int maxLiveCount = 3;

    public int LiveCount => liveCount;

    public void RemoveLife()
    {
        liveCount -= 1;

        if(liveCount <= 0)
        {
            liveCount = 0;
        }
    }
    #endregion

    #region GLOBAL

    public void Initialize(StatsData data)
    {
        hp = data.HP;
        shield = data.Shield;
        liveCount = data.LifeCount;
    }

    #endregion
}

[System.Serializable]
public class StatsData
{
    public float HP;
    public int Shield;
    public int LifeCount;
}
