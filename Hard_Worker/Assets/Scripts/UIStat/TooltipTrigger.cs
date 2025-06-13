using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 스텟창에 능력치별 아이콘에 스크립트를 등록한 뒤, 아이콘 마다 하단에 할당되어 있는 툴팁을 가져와서 마우스를 인식하여
/// 마우스에 닿으면 툴팁을 true로 바꿔 스텟의 설명을 볼 수 있게 만드는 툴팁 트리거 스크립트 입니다.
/// </summary>
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 툴팁을 직접 드래그해서 넣을 수 있게 필드 선언
    [SerializeField] private GameObject tooltipObject;


    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipObject.SetActive(false);
    }
}