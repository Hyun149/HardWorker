using System;
using UnityEngine;

/// <summary>
/// 무기 장착을 관리하는 매니저 클래스입니다.
/// - 장착 무기 저장/불러오기
/// - 기본 무기 생성
/// - 장착 시 이벤트 및 UI 갱신 처리
/// </summary>
public class WeaponManager : MonoBehaviour
{
    public event Action OnWeaponEquipped;   //착용 이벤트 
    
    [Header("무기")]
    [SerializeField] WeaponDataSO defaultWeaponData;  // 기본무기

    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private WeaponStatusUI weaponStatusUI;

    private Weapon equippedWeapon;

    /// <summary>
    /// 게임 시작 시 저장된 무기를 복원하거나 기본 무기를 장착합니다.
    /// </summary>
    private void Start()
    {
        string equippedId = GameManager.Instance.playerData.equippedWeaponId;
        Weapon equipped = weaponInventory.GetWeapons().Find(w => w.GetData().id == equippedId);

        if (equipped != null)
        {
            EquipWeapon(equipped); // 저장된 장착 무기 복원
        }
        else
        {
            // 장착 정보 없거나 무기 목록에 없으면 기본 무기 생성
            Weapon defaultWeapon = new Weapon(defaultWeaponData);
            weaponInventory.AddWeapon(defaultWeaponData);
            EquipWeapon(defaultWeapon);
        }
    }
    /// <summary>
    /// 장비장착시
    /// </summary>
    public void EquipWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
        OnWeaponEquipped?.Invoke();
        weaponStatusUI.DisplayWeapon(weapon); // UI 자동 연결
    }

    /// <summary>
    /// 현재 장착된 무기를 반환합니다.
    /// </summary>
    public Weapon GetEquippedWeapon() => equippedWeapon;
}
