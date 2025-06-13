using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 능력치 강화 버튼 클릭 시 StatCanvas를 활성화하는 스크립트입니다.
/// </summary>
public class StatButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject statCanvas;
    [SerializeField] private Button backButton;
    [SerializeField] private Button statButton;

    private StatUI StatUI;

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
        StatUI.RefreshUI();
        statCanvas.SetActive(true);
        gameCanvas.SetActive(false);
    }

    private void HideStatCanvas()
    {
        gameCanvas.SetActive(true);
        statCanvas.SetActive(false);
    }
}
