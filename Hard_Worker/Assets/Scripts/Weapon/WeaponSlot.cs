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
    public GameObject infoText;
    [Header("Buttons")]
    public Button buyButton;
    public Button upgradeButton;
    public Button equipButton;

    private Weapon weapon;
    private WeaponDataSO data;
    
    /// <summary>
    /// Slot 세팅: 가진거 or 기본무기일때 , 구매하지 않은 무기일때 나눠서 조건문탐
    /// </summary>
    public void SetupSlot(WeaponDataSO dataSO, Weapon ownedWeapon)
    {
        //Debug.Log($"Setting up slot for {dataSO.weaponName}, owned: {ownedWeapon != null}");
        data = dataSO;
        weapon = ownedWeapon;
        
        bool isOwned = weapon != null;
        bool isEquipped = isOwned && WeaponManager.Instance.GetEquippedWeapon() == weapon;
        bool isDefault = data.id == "0";

        icon.sprite = data.icon;
        AdjustIconSize(data.icon);

        if (isOwned || isDefault)
        {
            Debug.Log("dd");
            icon.color = Color.white;
            hiddenText.gameObject.SetActive(false);
            
            infoText.SetActive(true);
            nameText.text = data.weaponName;
            descriptionText.text = data.description;
            attackValue.text = weapon.GetAttack().ToString();
            criticalValue.text = weapon.GetCriticalRate().ToString() + "%";

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
            Debug.Log("d2");
            icon.color = new Color(0, 0, 0, 0.5f);
            hiddenText.text = "???";
            hiddenText.gameObject.SetActive(true);
            
            infoText.SetActive(false);
            nameText.text = "???";
            descriptionText.text = "";
            attackValue.text = "";
            criticalValue.text = "";

            buyButton.gameObject.SetActive(true);
            upgradeButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => TryPurchase());
        }
    }
    
    
    /// <summary>
    /// 처음 구입시 해당 무기의 강화테이블 레벨0일때의 cost을 내고 구입가능
    /// </summary>
    void TryPurchase()
    {
        // if (SkillPointManager.Instance.HasEnough(data.enhancementTable[0].cost))
        // {
        //     //SkillPointManager.Instance.SpendSP(data.enhancementTable[0].cost);
        //     WeaponInventory.Instance.AddWeapon(data);
        //     FindObjectOfType<WeaponInventoryUI>().RenderInventory();
        // }
        // else
        // {
        //     Debug.Log("Not enough SP");
        // }
        WeaponInventory.Instance.AddWeapon(data);
        FindObjectOfType<WeaponInventoryUI>().RenderInventory();
    }
    
    
    /// <summary>
    /// 처음 구입시 해당 무기의 강화테이블 레벨0의 비용을 내고 구입가능
    /// </summary>
    void TryEnhance()
    {
        // int cost = weapon.GetEnhanceCost();
        // if (SkillPointManager.Instance.HasEnough(cost))
        // {
        //     SkillPointManager.Instance.SpendSP(cost);
        //     weapon.Enhance();
        //     attackValue.text = weapon.GetAttack().ToString();
        //     criticalValue.text = weapon.GetCriticalRate().ToString()+"%";
        // }
        // else
        // {
        //     Debug.Log("Not enough SP");
        // }
        weapon.Enhance();
        attackValue.text = weapon.GetAttack().ToString();
        criticalValue.text = weapon.GetCriticalRate().ToString()+"%";
    }
    
    /// <summary>
    /// Icon 크기 조절
    /// </summary>
    /// <param name="sprite">무기 데이터의 아이콘데이터</param>
    private void AdjustIconSize(Sprite sprite)
    {
        if (sprite == null) return;

        RectTransform rt = icon.GetComponent<RectTransform>();

        // 원하는 최대 크기
        float maxSize = 120f;

        float w = sprite.rect.width;
        float h = sprite.rect.height;

        float scale = Mathf.Min(maxSize / w, maxSize / h);
        rt.sizeDelta = new Vector2(w * scale, h * scale);
    }
}
