using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Weapon equippedWeapon;
    [SerializeField] private WeaponStatusUI weaponStatusUI;
    public event Action OnWeaponEquipped;   //착용 이벤트 
    
    public void EquipWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
        OnWeaponEquipped?.Invoke();
        weaponStatusUI.DisplayWeapon(weapon); // UI 자동 연결
    }
    public Weapon GetEquippedWeapon() => equippedWeapon;
}
