using TMPro;
using UnityEngine;

public class WeaponInventoryUI : MonoBehaviour
{
    public Transform slotContainer; // SlotContainer
    public GameObject slotPrefab;
    public TextMeshProUGUI spAmountText;
    /// <summary>
    /// 무기 장착될때 이벤트 구독하여 자동 업뎃 
    /// </summary>
    private void OnEnable()
    {
        WeaponManager.Instance.OnWeaponEquipped += RenderInventory;
    }

    private void OnDisable()
    {
        WeaponManager.Instance.OnWeaponEquipped -= RenderInventory;
    }
    /// <summary>
    /// 인벤에서 장착,업글,구매 등 버튼을 눌렀을때호출, 슬롯들 UI를 업데이트해줌
    /// </summary>
    public void RenderInventory()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        foreach (var data in WeaponInventory.Instance.GetAllWeaponData())
        {
            var owned = WeaponInventory.Instance.GetWeapons().Find(w => w.GetData().id == data.id);
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            WeaponSlot slot = slotObj.GetComponent<WeaponSlot>();
            slot.SetupSlot(data, owned);
        }

        spAmountText.text = SkillPointManager.Instance.currentSP.ToString();
    }
}
