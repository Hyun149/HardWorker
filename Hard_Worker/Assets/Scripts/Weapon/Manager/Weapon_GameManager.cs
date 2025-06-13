using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_GameManager : MonoBehaviour
{
    public static Weapon_GameManager Instance;

    [Header("무기")]
    public WeaponDataSO defaultWeaponData;  // 기본무기

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 기본 무기 생성 및 등록
        Weapon defaultWeapon = new Weapon(defaultWeaponData);
        WeaponInventory.Instance.AddWeapon(defaultWeaponData);
        WeaponManager.Instance.EquipWeapon(defaultWeapon);
    }
}
