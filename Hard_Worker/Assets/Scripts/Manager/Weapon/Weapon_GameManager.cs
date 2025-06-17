using UnityEngine;

/// <summary>
/// 게임 시작 시 기본 무기를 생성하고 장착하는 초기화 매니저입니다.
/// - 무기 인벤토리와 장착 시스템을 초기화합니다.
/// </summary>
public class Weapon_GameManager : MonoBehaviour
{
    [Header("무기")]
    public WeaponDataSO defaultWeaponData;  // 기본무기

    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private WeaponManager weaponManager;

    /// <summary>
    /// 게임 시작 시 기본 무기를 생성하고 장착합니다.
    /// </summary>
    private void Start()
    {
        // 기본 무기 생성 및 등록
        Weapon defaultWeapon = new Weapon(defaultWeaponData);
        weaponInventory.AddWeapon(defaultWeaponData);
        weaponManager.EquipWeapon(defaultWeapon);
    }
}
