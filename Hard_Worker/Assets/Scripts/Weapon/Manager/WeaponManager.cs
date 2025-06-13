using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private Weapon equippedWeapon;

    public void Awake() => Instance = this;
    public void EquipWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
        WeaponStatusUI.Instance.DisplayWeapon(weapon); // UI 자동 연결
    }
    public Weapon GetEquippedWeapon() => equippedWeapon;
}
