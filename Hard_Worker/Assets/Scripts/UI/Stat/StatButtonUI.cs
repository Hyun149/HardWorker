using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 능력치 강화 UI 버튼 제어 스크립트입니다.
/// - 능력치 강화 버튼 클릭 시 StatCanvas를 활성화합니다.
/// - 뒤로 가기 버튼 클릭 시 게임 화면으로 돌아갑니다.
/// </summary>
public class StatButtonUI : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject statCanvas;
    [SerializeField] private Button backButton;
    [SerializeField] private Button statButton;
    [SerializeField] private StatUIManager uiManager;

    /// <summary>
    /// 버튼 클릭 이벤트를 등록합니다.
    /// </summary>
    private void Awake()
    {
        statButton.onClick.AddListener(ShowStatCanvas);
        backButton.onClick.AddListener(HideStatCanvas);
    }

   

    /// <summary>
    /// StatCanvas를 활성화합니다.
    /// </summary>
    private void ShowStatCanvas()
    {
        cursorManager.OnOtherUIOpen();
        SFXManager.Instance.Play(SFXType.UIShow);

        uiManager.InitializeAll();
        uiManager.RefreshAllUI();

        statCanvas.SetActive(true);
        gameCanvas.SetActive(false);
    }

    /// <summary>
    /// StatCanvas를 비활성화하고 게임Canvas를 다시 활성화합니다.
    /// - 커서를 게임모드로 복귀
    /// - 효과음 재생
    /// </summary>
    private void HideStatCanvas()
    {
        cursorManager.OnOtherUIClose();
        SFXManager.Instance.Play(SFXType.UIShow);

        gameCanvas.SetActive(true);
        statCanvas.SetActive(false);
    }
}
