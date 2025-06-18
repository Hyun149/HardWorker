using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// WeaponInventory : 플레이어가 보유한 무기를 관리하는 인벤토리 클래스
/// - 무기 데이터 SO 목록을 기반으로 실제 보유 중인 Weapon 인스턴스를 구성
/// - Add, Load 등의 기능을 통해 무기 획득 및 저장 데이터를 관리
/// </summary>
public class WeaponInventory : MonoBehaviour
{
    public List<WeaponDataSO> allWeaponData;

    private List<Weapon> ownedWeapons = new();
    
    public List<Weapon> GetWeapons() => ownedWeapons;
    public List<WeaponDataSO> GetAllWeaponData() => allWeaponData;

    /// <summary>
    /// 무기 데이터를 받아 해당 무기를 보유 목록에 추가합니다.
    /// - 중복 획득 방지 처리 포함
    /// - PlayerData에도 저장용 데이터(WeaponSaveData) 추가
    /// </summary>
    /// <param name="data">획득한 무기의 데이터 SO</param>
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
    /// PlayerData의 저장된 무기 목록을 기반으로 보유 무기를 구성합니다.
    /// - 저장된 강화 레벨까지 반영하여 Weapon 인스턴스를 복원합니다.
    /// </summary>
    /// <param name="playerData">저장된 플레이어 데이터</param>
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
        }
    }
}
