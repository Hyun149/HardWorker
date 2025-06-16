using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [Header("커서 이미지 UI")]
    public Image cursorImage;

    [Header("무기 매니저")]
    public WeaponManager weaponManager;
    
    [Header("애니메이션")]
    [SerializeField] private Animator cursorAnimator;
    
    private bool isOtherUIOpen = false;

    void Start()
    {
        // 시스템 커서를 투명하게 설정 (시작 시 한 번만)
        Cursor.SetCursor(CreateInvisibleCursor(), Vector2.zero, CursorMode.Auto);
        Cursor.visible = false;

        if (cursorImage != null)
            cursorImage.raycastTarget = false;

        if (weaponManager != null)
        {
            weaponManager.OnWeaponEquipped += UpdateCursorSprite;
            UpdateCursorSprite();
        }
    }

    void Update()
    {
        if (isOtherUIOpen || cursorImage == null || !cursorImage.enabled || cursorImage.canvas == null)
            return;
        UpdateCursorPosition();
    }

    void UpdateCursorSprite()
    {
        if (isOtherUIOpen) return;

        Weapon weapon = weaponManager.GetEquippedWeapon();
        if (weapon != null)
        {
            Sprite icon = weapon.GetData().icon;
            if (icon != null)
            {
                cursorImage.sprite = icon;
                cursorImage.SetNativeSize();
            }
        }
    }

    void UpdateCursorPosition()
    {
        if (cursorImage == null) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cursorImage.canvas.transform as RectTransform,
            Input.mousePosition,
            cursorImage.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cursorImage.canvas.worldCamera,
            out pos
        );

        cursorImage.rectTransform.anchoredPosition = pos;
    }

    /// <summary>
    /// 완전 투명한 시스템 커서용 텍스처
    /// </summary>
    Texture2D CreateInvisibleCursor()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, new Color(0, 0, 0, 0));
        tex.Apply();
        return tex;
    }

    /// <summary>
    /// 다른 UI 열릴때 호출
    /// </summary>
    public void OnOtherUIOpen()
    {
        isOtherUIOpen = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); //기본커서
        Cursor.visible = true;
        if (cursorImage != null) cursorImage.enabled = false;
    }

    /// <summary>
    /// 다른 UI 닫힐때 호출
    /// </summary>
    public void OnOtherUIClose()
    {
        isOtherUIOpen = false;
        
        Cursor.SetCursor(CreateInvisibleCursor(), Vector2.zero, CursorMode.Auto); //무기커서
        Cursor.visible = false;
        if (cursorImage != null)
        {
            cursorImage.enabled = true;
            UpdateCursorSprite();
        }
    }
    /// <summary>
    /// 클릭 애니메이션 플레이
    /// </summary>
    public void PlayClickAnimation()
    {
        if (cursorAnimator != null)
        {
            cursorAnimator.SetTrigger("Click");
        }
    }
}
