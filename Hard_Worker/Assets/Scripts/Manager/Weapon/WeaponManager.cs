using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("무기")]
    public WeaponDataSO defaultWeaponData;  // 기본무기

    [SerializeField] private WeaponInventory weaponInventory;
    
    private Weapon equippedWeapon;
    [SerializeField] private WeaponStatusUI weaponStatusUI;
    public event Action OnWeaponEquipped;   //착용 이벤트 
    private void Start()
    {
        // 기본 무기 생성 및 등록
        Weapon defaultWeapon = new Weapon(defaultWeaponData);
        weaponInventory.AddWeapon(defaultWeaponData);
        EquipWeapon(defaultWeapon);
    }
    public void EquipWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
        OnWeaponEquipped?.Invoke();
        weaponStatusUI.DisplayWeapon(weapon); // UI 자동 연결
    }
    public Weapon GetEquippedWeapon() => equippedWeapon;
}
