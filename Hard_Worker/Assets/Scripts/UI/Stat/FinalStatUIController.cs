using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 최종 스탯 패널의 열기/닫기를 제어하는 UI 컨트롤러입니다.
/// - 버튼 클릭으로 패널을 표시하거나 숨깁니다.
/// - 커서 상태 및 사운드 효과도 함께 제어합니다.
/// </summary>
public class FinalStatUIController : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private Button finalStatButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject finalStatPanel;
    [SerializeField] private UIAnimator finalStatPanelAnimator;
    [SerializeField] private FinalStatUI finalStatUI;

    /// <summary>
    /// 버튼 이벤트를 초기화합니다.
    /// </summary>
    private void Awake()
    {
        finalStatButton.onClick.AddListener(OpenFinalStatPanel);
        backButton.onClick.AddListener(CloseFinalStatPanel);
    }

    /// <summary>
    /// 최종 스탯 패널을 엽니다.
    /// 커서 UI 모드로 전환하고 효과음을 재생한 뒤 애니메이션을 실행합니다.
    /// </summary>
    private void OpenFinalStatPanel()
    {
        cursorManager.OnOtherUIOpen();
        SFXManager.Instance.Play(SFXType.UIShow);
        finalStatPanelAnimator.Show();
        finalStatUI.RefreshAllUI();
    }

    /// <summary>
    /// 최종 스탯 패널을 닫습니다.
    /// 커서를 게임 모드로 전환하고 효과음을 재생한 뒤 애니메이션을 종료합니다.
    /// </summary>
    private void CloseFinalStatPanel()
    {
        cursorManager.OnOtherUIClose();
        SFXManager.Instance.Play(SFXType.UIShow);
        finalStatPanelAnimator.Hide();
    }
}
