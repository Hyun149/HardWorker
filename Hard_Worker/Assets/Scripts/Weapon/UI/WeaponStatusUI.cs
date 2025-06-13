using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatusUI : MonoBehaviour
{
    public static WeaponStatusUI Instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI criticalText;
    public Image icon;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayWeapon(Weapon weapon)
    {
        var data = weapon.GetData();
        nameText.text = data.weaponName;
        descriptionText.text = data.description;
        attackText.text = $"{weapon.GetAttack()}";
        criticalText.text = $"{weapon.GetCriticalRate()}%";
        icon.sprite = data.icon;
        icon.preserveAspect = true;
        AdjustIconSize(data.icon);
    }
    
    
    /// <summary>
    /// Icon 크기 조절
    /// </summary>
    /// <param name="sprite"></param>
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
