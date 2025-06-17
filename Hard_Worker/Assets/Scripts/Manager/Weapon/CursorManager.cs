using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 무기 아이콘 기반 커서 시스템을 관리하는 클래스입니다.
/// - 무기 장착 시 커서 이미지 변경
/// - 커서 위치를 마우스에 따라 실시간 갱신
/// - UI 열림 여부에 따라 커서 표시 방식 전환
/// </summary>
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

        // 커서 이미지에 Raycast 안 먹히게 설정
        if (cursorImage != null)
            cursorImage.raycastTarget = false;

        // 무기 장착 시 커서 아이콘 자동 업데이트
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

    /// <summary>
    /// 현재 장착된 무기의 아이콘으로 커서 이미지를 업데이트합니다.
    /// </summary>
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

    /// <summary>
    /// 커서 이미지를 마우스 위치에 맞게 이동시킵니다.
    /// </summary>
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
