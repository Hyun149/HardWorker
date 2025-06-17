using UnityEngine;
using TMPro;
public class AutoAttackTester : MonoBehaviour
{
    [Header("테스트 설정")]
    [SerializeField] private int testPlayerLevel = 1;
    [SerializeField] private ClickEventHandler clickEventHandler;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI autoAttackStatusText;

    // 자동 공격 관련 상태를 추적하기 위한 변수들
    private int currentAutoAttackLevel = 0;
    private bool currentPauseState = false;

    void Start()
    {
        if (clickEventHandler == null)
        {
            clickEventHandler = FindObjectOfType<ClickEventHandler>();
        }
        UpdateUI();
    }
    void Update()
    {
        // 테스트용 키보드 입력
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeAutoAttack();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }
    // 레벨업 테스트
    void LevelUp()
    {
        testPlayerLevel++;
        clickEventHandler.SetPlayerLevel(testPlayerLevel);
        UpdateUI();
    }
    // 자동 공격 업그레이드 테스트
    void UpgradeAutoAttack()
    {
        currentAutoAttackLevel++;
        clickEventHandler.SetAutoAttackLevel(currentAutoAttackLevel);
        UpdateUI();
    }
    // 일시정지 토글
    void TogglePause()
    {
        currentPauseState = !currentPauseState;
        clickEventHandler.SetPause(currentPauseState);
        UpdateUI();
    }
    // UI 업데이트
    void UpdateUI()
    {
        if (playerLevelText != null)
        {
            playerLevelText.text = $"플레이어 레벨: {testPlayerLevel}";
        }
        if (autoAttackStatusText != null)
        {
            string status = "";
            if (clickEventHandler.IsAutoAttackUnlocked())
            {
                status += $"자동 공격: 해금됨\n";
                status += $"자동 공격 레벨: {currentAutoAttackLevel}\n";
                status += $"공격 간격: {clickEventHandler.GetAutoAttackInterval():F2}초\n";
                status += $"일시정지: {(currentPauseState ? "활성" : "비활성")}";
            }
            else
            {
                status = $"자동 공격: 레벨 10에 해금"; // 기본값으로 10 사용
            }
            autoAttackStatusText.text = status;
        }
    }
    void OnGUI()
    {
        // 테스트용 GUI
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== 자동 공격 테스트 ===");
        GUILayout.Label("L키: 레벨업");
        GUILayout.Label("U키: 자동 공격 업그레이드");
        GUILayout.Label("P키: 일시정지 토글");
        GUILayout.Label("마우스 클릭: 수동 공격");
        GUILayout.EndArea();
    }
}