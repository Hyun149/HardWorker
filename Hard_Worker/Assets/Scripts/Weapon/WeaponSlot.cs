using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// WeaponSlot : 무기 UI 슬롯을 구성하고 상호작용을 처리하는 클래스
/// - 아이콘, 텍스트, 버튼 등을 통해 무기 정보를 출력
/// - 무기 보유 여부, 강화 가능 여부, 장착 여부에 따라 조건 분기하여 UI 표시
/// - 구매, 강화, 장착 시 기능 실행 및 게임 상태 업데이트
/// </summary>
public class WeaponSlot : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI hiddenText;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI attackValue;
    [SerializeField] private TextMeshProUGUI criticalValue;
    [SerializeField] private GameObject infoText;

    [Header("InteractContainer")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private TextMeshProUGUI maxUpgradeText;
    [SerializeField] private TextMeshProUGUI[] requireSPTexts;

    private WeaponInventory weaponInventory;
    private SkillPointManager skillPointManager;
    private WeaponManager weaponManager;
    private WeaponStatusUI weaponStatusUI;
    
    private Weapon weapon;
    private WeaponDataSO data;

    /// <summary>
    /// 슬롯에 필요한 의존성 컴포넌트를 외부에서 주입합니다.
    /// </summary>
    /// <param name="inventory">WeaponInventory 인스턴스</param>
    /// <param name="skillManager">SkillPointManager 인스턴스</param>
    /// <param name="manager">WeaponManager 인스턴스</param>
    /// <param name="weaponStatus">WeaponStatusUI 인스턴스</param>
    public void SetDependencies(WeaponInventory inventory, SkillPointManager skillManager,WeaponManager manager,WeaponStatusUI weaponStatus)
    {
        this.weaponInventory = inventory;
        this.skillPointManager = skillManager;
        this.weaponManager = manager;
        this.weaponStatusUI = weaponStatus;
    }

    /// <summary>
    /// 무기 슬롯을 초기화하여 보유/미보유 상태에 따라 UI를 구성합니다.
    /// </summary>
    /// <param name="dataSO">무기 데이터 (ScriptableObject)</param>
    /// <param name="ownedWeapon">플레이어가 보유 중인 무기 인스턴스 (없으면 null)</param>
    public void SetupSlot(WeaponDataSO dataSO, Weapon ownedWeapon)
    {
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
            // 보유 상태
            icon.color = Color.white;
            hiddenText.gameObject.SetActive(false);
            
            infoText.SetActive(true);
            nameText.text = data.weaponName;
            descriptionText.text = data.description;
            //weapon매니저보다 인벤토리 슬롯이 먼저 실행함 따라서 아래서 분기설정
            if (weapon != null) //기본무기가 할당이 안되면
            {
                attackValue.text = weapon.GetAttack().ToString();
                criticalValue.text = weapon.GetCriticalRate().ToString() + "%";
            }
            else //기본무기가 할당되면
            {
                attackValue.text = "5";
                criticalValue.text = "1";
            }

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
            // 미보유 상태
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
    /// 무기 최초 구매 처리: SP 소모 후 무기 인벤토리에 추가하고 저장
    /// </summary>
    void TryPurchase()
    {
        if (skillPointManager.HasEnough(data.enhancementTable[0].cost))
        {
            skillPointManager.SpendSP(data.enhancementTable[0].cost);
            weaponInventory.AddWeapon(data);

            var saveList = GameManager.Instance.playerData.ownedWeapons;
            if (!saveList.Exists(w => w.weaponId == data.id))
            {
                saveList.Add(new WeaponSaveData(data.id, 0, 0));
            }

            FindObjectOfType<WeaponInventoryUI>().RenderInventory();

            SFXManager.Instance.Play(SFXType.Buy);
            GameManager.Instance.SaveGame();
        }
    }


    /// <summary>
    /// 무기 강화 처리: SP 소모 후 강화 진행 및 UI, 세이브 데이터 갱신
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

            WeaponSaveData saveData = GameManager.Instance.playerData.ownedWeapons.Find(w => w.weaponId == weapon.GetData().id);

            if (saveData != null)
            {
                saveData.enhanceLevel = weapon.GetLevel();
                GameManager.Instance.SaveGame();
            }
        }
    }

    /// <summary>
    /// 무기 장착 처리: 장착 후 저장 및 능력치 갱신
    /// </summary>
    void TryEquip()
    {
        SFXManager.Instance.Play(SFXType.EquipWeapon); // 장착 사운드
        weaponManager.EquipWeapon(weapon);

        GameManager.Instance.playerData.equippedWeaponId = weapon.GetData().id;
        GameManager.Instance.SaveGame();

        FindObjectOfType<PlayerStat>()?.NotifyStatChanged();
    }

    /// <summary>
    /// 무기 아이콘의 크기를 자동으로 조절합니다.
    /// </summary>
    /// <param name="sprite">무기 아이콘 스프라이트</param>
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
