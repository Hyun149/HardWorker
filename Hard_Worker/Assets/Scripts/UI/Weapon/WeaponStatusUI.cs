using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatusUI : MonoBehaviour
{
    public static WeaponStatusUI Instance;
    
    [Header("장착 무기 정보")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI criticalText;
    public Image icon;
    
    [Header("인벤토리버튼")]
    public Button inventoryButton;
    private Image buttonimage;
    public List<Sprite> buttonImages;
    private TextMeshProUGUI buttonText;
    public WeaponInventoryUI inventoryUI;
    
    [SerializeField] private CursorManager cursorManager;
    void Start()
    {
        inventoryButton.onClick.AddListener(OnClickInventoryButton);
        buttonimage = inventoryButton.GetComponent<Image>();
        buttonText = inventoryButton.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    /// <summary>
    /// 장착무기 정보 표시
    /// </summary>
    /// <param name="weapon">UI에 표시할 대상 무기 인스턴스</param>
    public void DisplayWeapon(Weapon weapon)
    {
        var data = weapon.GetData();
        nameText.text = data.weaponName;
        attackText.text = $"{weapon.GetAttack()}";
        criticalText.text = $"{weapon.GetCriticalRate()}%";
        icon.sprite = data.icon;
        icon.preserveAspect = true;
        AdjustIconSize(data.icon);
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
    
    /// <summary>
    /// 인벤토리 버튼 클릭시
    /// </summary>
    void OnClickInventoryButton()
    {
        Transform panel = inventoryUI.transform.Find("Panel");

        if (panel != null)
        {
            bool isActive = !panel.gameObject.activeSelf;
            panel.gameObject.SetActive(isActive);
            inventoryUI.RenderInventory();
            
            buttonimage.sprite = isActive ? buttonImages[1] : buttonImages[0];
            buttonText.text = isActive ? "도구 가방 접기" : "도구 가방 열기";

            if (isActive)
                cursorManager.OnOtherUIOpen(); // 인벤토리 열림 → 기본 커서
            else
                cursorManager.OnOtherUIClose();
            
            SFXManager.Instance.Play(SFXType.UIShow); // 열고 접는 사운드
        }
    }
}
