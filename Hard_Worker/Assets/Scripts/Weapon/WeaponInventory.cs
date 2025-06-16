using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<WeaponDataSO> allWeaponData;
    private List<Weapon> ownedWeapons = new();
    
    public List<Weapon> GetWeapons() => ownedWeapons;
    public List<WeaponDataSO> GetAllWeaponData() => allWeaponData;
    
    /// <summary>
    /// 무기를 현재 자신의 가진 무기리스트로 추가
    /// </summary>
    /// <param name="data">클릭한 무기 인스턴스 데이터</param>
    public void AddWeapon(WeaponDataSO data)
    {
        if (!ownedWeapons.Any(w => w.GetData().id == data.id))
        {
            ownedWeapons.Add(new Weapon(data));
        }
        else
        {
            Debug.Log("Weapon already exists, not added.");
        }

    }
}
