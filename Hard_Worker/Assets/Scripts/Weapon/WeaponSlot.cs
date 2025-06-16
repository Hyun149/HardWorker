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
    [Header("InteractContainer")]
    public Button buyButton;
    public Button upgradeButton;
    public Button equipButton;
    public TextMeshProUGUI maxUpgradeText;
    public TextMeshProUGUI[] requireSPTexts;

    private WeaponInventory weaponInventory;
    private SkillPointManager skillPointManager;
    private WeaponManager weaponManager;
    private WeaponStatusUI weaponStatusUI;
    
    private Weapon weapon;
    private WeaponDataSO data;
    /// <summary>
    /// 슬롯 렌더링할때 슬롯스크립트에 inventory,skillpointmanager 주입
    /// </summary>
    public void SetDependencies(WeaponInventory inventory, SkillPointManager skillManager,WeaponManager manager,WeaponStatusUI weaponStatus)
    {
        this.weaponInventory = inventory;
        this.skillPointManager = skillManager;
        this.weaponManager = manager;
        this.weaponStatusUI = weaponStatus;
    }
    
    /// <summary>
    /// Slot 세팅: 가진거 or 기본무기일때 , 구매하지 않은 무기일때 나눠서 조건문탐
    /// </summary>
    public void SetupSlot(WeaponDataSO dataSO, Weapon ownedWeapon)
    {
        //Debug.Log($"Setting up slot for {dataSO.weaponName}, owned: {ownedWeapon != null}");
        data = dataSO;
        weapon = ownedWeapon;
        
        bool isOwned = weapon != null;
        bool isEquipped = isOwned && weaponManager.GetEquippedWeapon()?.GetData().id == data.id;
        bool isDefault = data.id == "0";

        icon.sprite = data.icon;
        AdjustIconSize(data.icon);
        
        int costToShow = 0;
        if (isOwned)
            costToShow = weapon.GetEnhanceCost();
        else
            costToShow = data.enhancementTable[0].cost; // 0레벨 강화비용 (구매 비용) 사용

        foreach (var text in requireSPTexts)
            text.text = $"X {costToShow}";
        
        upgradeButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        
        if (isOwned || isDefault)
        {
            icon.color = Color.white;
            hiddenText.gameObject.SetActive(false);
            
            infoText.SetActive(true);
            nameText.text = data.weaponName;
            descriptionText.text = data.description;
            attackValue.text = weapon.GetAttack().ToString();
            criticalValue.text = weapon.GetCriticalRate().ToString() + "%";

            buyButton.gameObject.SetActive(false);
            
            bool isMaxEnhanced = isOwned && weapon.GetLevel() >= data.enhancementTable.Count - 1;
    
            upgradeButton.gameObject.SetActive(!isMaxEnhanced);
            maxUpgradeText.gameObject.SetActive(isMaxEnhanced);

            if (!isMaxEnhanced)
            {
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(() => TryEnhance());
            }
            equipButton.onClick.RemoveAllListeners();
            if (!isEquipped)
            {
                equipButton.gameObject.SetActive(true);
                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(() => TryEquip());
            }
        }
        else
        {
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
        if (skillPointManager.HasEnough(data.enhancementTable[0].cost))
        {
            skillPointManager.SpendSP(data.enhancementTable[0].cost);
            weaponInventory.AddWeapon(data);
            FindObjectOfType<WeaponInventoryUI>().RenderInventory();

            SFXManager.Instance.Play(SFXType.Buy);
        }
        else
        {
            Debug.Log("Not enough SP");
        }
    }
    
    
    /// <summary>
    /// 처음 구입시 해당 무기의 강화테이블 레벨0의 비용을 내고 구입가능
    /// </summary>
    void TryEnhance()
    {
        int cost = weapon.GetEnhanceCost();
        if (skillPointManager.HasEnough(cost))
        {
            skillPointManager.SpendSP(cost);
            weapon.Enhance();
            FindObjectOfType<WeaponInventoryUI>().RenderInventory();
            Weapon equippedWeapon = weaponManager.GetEquippedWeapon();
            if (equippedWeapon != null && equippedWeapon == weapon)
            {
                weaponStatusUI.DisplayWeapon(weapon);
            }

            SFXManager.Instance.Play(SFXType.WeaponEnhance);
        }
        else
        {
            Debug.Log("Not enough SP");
        }
    }
    /// <summary>
    /// 무기 장착시 
    /// </summary>
    void TryEquip()
    {
        SFXManager.Instance.Play(SFXType.EquipWeapon); // 장착 사운드
        weaponManager.EquipWeapon(weapon);
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
