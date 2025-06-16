using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 능력치 강화 버튼 클릭 시 StatCanvas를 활성화하는 스크립트입니다.
/// </summary>
public class StatButtonUI : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject statCanvas;
    [SerializeField] private Button backButton;
    [SerializeField] private Button statButton;
    [SerializeField] private StatUIManager uiManager;


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

    private void HideStatCanvas()
    {
        cursorManager.OnOtherUIClose();
        SFXManager.Instance.Play(SFXType.UIShow);

        gameCanvas.SetActive(true);
        statCanvas.SetActive(false);
    }
}
