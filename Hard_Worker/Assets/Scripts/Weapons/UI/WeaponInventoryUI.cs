using TMPro;
using UnityEngine;

/// <summary>
/// WeaponInventoryUI : 무기 인벤토리 UI를 관리하는 클래스입니다.
/// - 슬롯 프리팹을 기반으로 현재 보유 중인 무기를 동적으로 표시합니다.
/// - 무기 장착/구매/강화 후 UI를 자동으로 갱신합니다.
/// - 무기 장착 시 WeaponManager 이벤트를 구독하여 동기화 처리합니다.
/// </summary>
public class WeaponInventoryUI : MonoBehaviour
{
    [Header("UI 구성요소")]
    [Tooltip("무기 슬롯이 배치될 부모 컨테이너")]
    [SerializeField] private Transform slotContainer;

    [Tooltip("무기 슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Tooltip("보유 중인 스킬 포인트 텍스트")]
    [SerializeField] private TextMeshProUGUI spAmountText;

    [Header("의존성")]
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private SkillPointManager skillPointManager;
    [SerializeField] private WeaponStatusUI weaponStatusUI;
    [SerializeField] private CursorManager cursorManager;

    /// <summary>
    /// 시작 시 저장된 무기 데이터를 불러오고 인벤토리 UI를 렌더링합니다.
    /// </summary>
    private void Start()
    {
        weaponInventory.LoadFromPlayerData(GameManager.Instance.playerData);

        RenderInventory();
    }

    /// <summary>
    /// 무기 장착될때 이벤트 구독하여 자동 업뎃 
    /// </summary>
    private void OnEnable()
    {
        weaponManager.OnWeaponEquipped += RenderInventory;
    }

    /// <summary>
    /// 이벤트 구독을 해제하여 메모리 누수나 중복 호출을 방지합니다.
    /// </summary>
    private void OnDisable()
    {
        weaponManager.OnWeaponEquipped -= RenderInventory;
    }

    /// <summary>
    /// 인벤토리를 다시 렌더링하여 슬롯을 갱신합니다.
    /// - 기존 슬롯들을 제거한 뒤, 현재 보유한 무기를 기준으로 슬롯을 재생성합니다.
    /// - 슬롯에는 무기 데이터, 소유 여부, 강화 상태가 반영됩니다.
    /// - 스킬 포인트도 최신 값으로 갱신됩니다.
    /// </summary>
    public void RenderInventory()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        foreach (var data in weaponInventory.GetAllWeaponData())
        {
            var owned = weaponInventory.GetWeapons().Find(w => w.GetData().id == data.id);
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            WeaponSlot slot = slotObj.GetComponent<WeaponSlot>();
            
            slot.SetDependencies(weaponInventory, skillPointManager,weaponManager,weaponStatusUI);
            
            slot.SetupSlot(data, owned);
        }

        spAmountText.text = GameManager.Instance.playerData.currentSkillPoint.ToString();
    }
}
