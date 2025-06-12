using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Weapon> owendWeapons;

    private List<WeaponData> allWeaponsDataList;

    public bool IsOwned(string id)
    {
        return true;
    }

    public void AddWeapon(WeaponData data)
    {
        
    }
}
