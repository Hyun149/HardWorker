using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [Header("Icon")]
    public Image icon;
    public TextMeshProUGUI hiddenText;

    [Header("Text")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attackValue;
    public TextMeshProUGUI criticalValue;

    [Header("Buttons")]
    public Button buyButton;
    public Button upgradeButton;
    public Button equipButton;

    private Weapon weapon;
    private WeaponDataSO data;
    
    /// <summary>
    /// Slot 세팅
    /// </summary>
    public void SetupSlot(WeaponDataSO dataSO, Weapon ownedWeapon)
    {
        data = dataSO;
        weapon = ownedWeapon;
        
        bool isOwned = weapon != null;
        bool isEquipped = isOwned && WeaponManager.Instance.GetEquippedWeapon() == weapon;
        bool isDefault = data.id == "0";

        icon.sprite = data.icon;

        if (isOwned || isDefault)
        {
            icon.color = Color.white;
            hiddenText.gameObject.SetActive(false);
            nameText.text = data.weaponName;
            descriptionText.text = data.description;
            attackValue.text = weapon.GetAttack().ToString();
            criticalValue.text = weapon.GetCriticalRate().ToString("P1");

            buyButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(!isEquipped);

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => TryEnhance());

            equipButton.onClick.RemoveAllListeners();
            if (!isEquipped)
                equipButton.onClick.AddListener(() => WeaponManager.Instance.EquipWeapon(weapon));
        }
        else
        {
            icon.color = new Color(0, 0, 0, 0.5f);
            hiddenText.text = "???";
            hiddenText.gameObject.SetActive(true);
            nameText.text = "";
            descriptionText.text = "";
            attackValue.text = "-";
            criticalValue.text = "-";

            buyButton.gameObject.SetActive(true);
            upgradeButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => TryPurchase());
        }
    }
    
    
    /// <summary>
    /// 처음 구입시 해당 무기의 강화테이블 레벨0의 비용을 내고 구입가능
    /// </summary>
    void TryPurchase()
    {
        if (SkillPointManager.Instance.HasEnough(data.enhancementTable[0].cost))
        {
            //SkillPointManager.Instance.SpendSP(data.enhancementTable[0].cost);
            WeaponInventory.Instance.AddWeapon(data);
            FindObjectOfType<WeaponInventoryUI>().RenderInventory();
        }
        else
        {
            Debug.Log("Not enough SP");
            //SoundManager.Instance.Play("NotEnoughSP");
        }
    }
    
    
    /// <summary>
    /// 처음 구입시 해당 무기의 강화테이블 레벨0의 비용을 내고 구입가능
    /// </summary>
    void TryEnhance()
    {
        int cost = weapon.GetEnhanceCost();
        if (SkillPointManager.Instance.HasEnough(cost))
        {
            SkillPointManager.Instance.SpendSP(cost);
            weapon.Enhance();
            attackValue.text = weapon.GetAttack().ToString();
            criticalValue.text = weapon.GetCriticalRate().ToString("P1");
        }
        else
        {
            Debug.Log("Not enough SP");
            //SoundManager.Instance.Play("NotEnoughSP");
        }
    }
}
