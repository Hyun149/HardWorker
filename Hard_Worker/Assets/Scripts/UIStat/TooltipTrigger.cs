using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// TooltipTrigger : 능력치 아이콘에 마우스 오버 시 툴팁을 표시하는 트리거 스크립트
/// - 스텟창 UI 아이콘에 부착하여, 마우스를 올리면 하단 툴팁이 활성화됨
/// </summary>
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("툴팁 오브젝트 연결")]
    // UI에서 드래그로 연결할 툴팁 오브젝트
    [SerializeField] private GameObject tooltipObject;

    /// <summary>
    /// 마우스가 아이콘 위에 올라갔을 때 호출
    /// 툴팁을 활성화하여 설명 표시
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 툴팁 ON
        tooltipObject.SetActive(true); 
    }

    /// <summary>
    /// 마우스가 아이콘에서 벗어났을 때 호출
    /// 툴팁을 비활성화하여 설명 숨김
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // 툴팁 OFF
        tooltipObject.SetActive(false);
    }
}