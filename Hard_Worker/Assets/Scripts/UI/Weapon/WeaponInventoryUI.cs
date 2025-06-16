using TMPro;
using UnityEngine;

public class WeaponInventoryUI : MonoBehaviour
{
    public Transform slotContainer; // SlotContainer
    public GameObject slotPrefab;
    public TextMeshProUGUI spAmountText;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private SkillPointManager skillPointManager;
    [SerializeField] private WeaponStatusUI weaponStatusUI;
    [SerializeField] private CursorManager cursorManager;

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
        //cursorManager.OnInventoryOpen();
    }

    private void OnDisable()
    {
        weaponManager.OnWeaponEquipped -= RenderInventory;
        //cursorManager.OnInventoryClose();
    }
    /// <summary>
    /// 인벤에서 장착,업글,구매 등 버튼을 눌렀을때호출, 슬롯들 UI를 업데이트해줌
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

        spAmountText.text = skillPointManager.currentSP.ToString();
    }
}
