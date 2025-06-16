using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [Header("무기")]
    public WeaponDataSO defaultWeaponData;  // 기본무기

    [SerializeField] private WeaponInventory weaponInventory;
    
    private Weapon equippedWeapon;
    [SerializeField] private WeaponStatusUI weaponStatusUI;
    public event Action OnWeaponEquipped;   //착용 이벤트 

    private Image icon;
    private void Start()
    {
        string equippedId = GameManager.Instance.playerData.equippedWeaponId;
        Weapon equipped = weaponInventory.GetWeapons().Find(w => w.GetData().id == equippedId);

        if (equipped != null)
        {
            EquipWeapon(equipped); // 저장된 장착 무기 복원
            Debug.Log($"[무기 복원] {equippedId} 장착됨");
        }
        else
        {
            // 장착 정보 없거나 무기 목록에 없으면 기본 무기 생성
            Weapon defaultWeapon = new Weapon(defaultWeaponData);
            weaponInventory.AddWeapon(defaultWeaponData);
            EquipWeapon(defaultWeapon);
            Debug.Log($"[기본 무기 장착]");
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
    public Weapon GetEquippedWeapon() => equippedWeapon;
}
