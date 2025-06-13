using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public WeaponDataSO data;
    private int enhanceLevel;
    
    public Weapon(WeaponDataSO data)
    {
        this.data = data;
        enhanceLevel = 0;
    }
    
    public int GetAttack() => data.enhancementTable[enhanceLevel].attack;
    public float GetCriticalRate() => data.enhancementTable[enhanceLevel].criticalRate;
    public int GetEnhanceCost() => data.enhancementTable[enhanceLevel].cost;
    public void Enhance() { if (enhanceLevel < data.enhancementTable.Count - 1) enhanceLevel++; }
    public int GetLevel() => enhanceLevel;
    public WeaponDataSO GetData() => data;
}
