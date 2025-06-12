using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData data;

    private int enhanceLevel;

    void Start()
    {
        
    }
    public Weapon(WeaponData data)
    {
        this.data = data;
    }
    
    public int GetAttack()
    {
        return 0;
    }

    public void TryEnhance()
    {
        
    }

    public float GetCriticalRate()
    {
        return 0;
    }
}
