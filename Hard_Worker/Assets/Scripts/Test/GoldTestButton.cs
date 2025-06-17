using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GoldTestButton : 개발/디버깅 용도로 골드를 즉시 추가하는 버튼 처리 클래스입니다.
/// - UI 버튼을 눌렀을 때 GoldManager를 통해 골드를 추가합니다.
/// - 테스트 및 밸런싱 확인 시 사용됩니다.
/// </summary>
public class GoldTestButton : MonoBehaviour
{
    [SerializeField] private Button addButton;

    /// <summary>
    /// 버튼 클릭 시 골드를 100,000만큼 추가하는 이벤트를 등록합니다.
    /// </summary>
    private void Awake()
    {
        addButton.onClick.AddListener(() => GoldManager.Instance.AddGold(100000));
    }
}
