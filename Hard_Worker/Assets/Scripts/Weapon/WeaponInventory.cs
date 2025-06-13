using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public static WeaponInventory Instance;
    
    public List<WeaponDataSO> allWeaponData;
    private List<Weapon> ownedWeapons = new();

    private void Awake() => Instance = this;
    
    public List<Weapon> GetWeapons() => ownedWeapons;
    public List<WeaponDataSO> GetAllWeaponData() => allWeaponData;
    public void AddWeapon(WeaponDataSO data)
    {
        if (!ownedWeapons.Any(w => w.GetData().id == data.id))
            ownedWeapons.Add(new Weapon(data));
    }
}
