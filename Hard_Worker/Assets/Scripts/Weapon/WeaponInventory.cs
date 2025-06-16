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
            Weapon newWeapon = new Weapon(data);
            ownedWeapons.Add(newWeapon);

            if (!GameManager.Instance.playerData.ownedWeapons.Any(w => w.weaponId == data.id))
            {
                GameManager.Instance.playerData.ownedWeapons.Add(new WeaponSaveData(data.id, 0, 0));
            }
        }
    }

    /// <summary>
    /// PlayerData의 저장된 무기 목록을 불러와 Weapon 인스턴스 구성
    /// </summary>
    public void LoadFromPlayerData(PlayerData playerData)
    {
        ownedWeapons.Clear();

        foreach (var save in playerData.ownedWeapons)
        {
            WeaponDataSO data = allWeaponData.Find(d => d.id == save.weaponId);
            if (data != null)
            {
                Weapon weapon = new Weapon(data);

                for (int i = 0; i < save.enhanceLevel; i++)
                {
                    weapon.Enhance();
                }

                ownedWeapons.Add(weapon);
            }
            else
            {
                Debug.LogWarning($"WeaponDataSO not found for id: {save.weaponId}");
            }
        }
    }
}
