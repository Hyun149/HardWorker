using UnityEngine;

public class Weapon_GameManager : MonoBehaviour
{
    [Header("무기")]
    public WeaponDataSO defaultWeaponData;  // 기본무기

    [SerializeField] private WeaponInventory weaponInventory;

    private void Start()
    {
        // 기본 무기 생성 및 등록
        Weapon defaultWeapon = new Weapon(defaultWeaponData);
        weaponInventory.AddWeapon(defaultWeaponData);
        WeaponManager.Instance.EquipWeapon(defaultWeapon);
    }
}
