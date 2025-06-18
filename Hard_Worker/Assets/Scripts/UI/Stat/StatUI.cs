using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// StatUI : 각 능력치 항목의 UI를 관리하는 클래스
/// - 능력치 수치, 강화 레벨, 업그레이드 비용을 표시하고
/// - 버튼 클릭 시 PlayerStat을 통해 능력치를 강화하고 UI를 갱신함
/// - 단발성으로 클릭 시 1회 강화
/// - 길게 누르면 0.5초 뒤부터 0.2초 간격으로 반복 강화
/// - 롱클릭은 Unity EventTrigger로 연결됨
/// </summary>
public class StatUI : MonoBehaviour
{
    // UI 연결 (Hierarchy에서 드래그 연결 필요)
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI valueText;  // 최종 능력치 값 표시
    [SerializeField] private TextMeshProUGUI costText;   // 업그레이드 비용 표시
    [SerializeField] private TextMeshProUGUI levelText;  // 현재 레벨 표시
    [SerializeField] private Button upgradeButton;       // 강화 버튼

    // 설정
    [Header("설정")]
    [SerializeField] private StatType statType;          // 이 UI가 관리하는 능력치 종류

    // 롱클릭 강화용 변수
    private Coroutine holdCoroutine;                     // 반복 실행 코루틴 참조
    private bool isHolding = false;                      // 현재 누르고 있는지 여부

    private PlayerStat playerStat;                       // 능력치 계산 및 저장 시스템 참조

    public StatType StatType => statType;                // 외부 접근용 프로퍼티


    /// <summary>
    /// StatType과 PlayerStat 시스템 연결 및 초기 UI 세팅
    /// </summary>
    public void Initialize(StatType type, PlayerStat statSystem)
    {
        statType = type;
        playerStat = statSystem;

        RefreshUI();

        // 버튼 클릭 시: 강화 → UI 갱신
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() =>
        {
            playerStat.UpgradeStat(statType);
            RefreshUI();
        });
    }

    /// <summary>
    /// 현재 능력치 수치, 레벨, 비용 정보를 가져와 UI에 출력
    /// </summary>
    public void RefreshUI()
    {
        int level = playerStat.GetStatLevel(statType);     // 현재 강화 레벨
        float value = playerStat.GetStatValue(statType);   // 최종 능력치 값
        int cost = playerStat.GetUpgradeCost(statType);    // 다음 업그레이드 비용

        // 능력치별 표시 형식 지정
        switch (statType)
        {
            case StatType.Income:
            case StatType.CritChance:
            case StatType.CritBonus:
                valueText.text = $"{value}%";              // 퍼센트 형식
                break;  

            case StatType.AssistSpeed:
                valueText.text = $"{value:0.0}s";          // 시간(초) 형식
                break;

            default:
                valueText.text = value.ToString();         // 일반 숫자 형식
                break;
        }

        // 비용 및 레벨 텍스트 갱신
        bool isMaxLevel = level >= playerStat.GetMaxLevel(statType);
        costText.text = (cost > 0 && !isMaxLevel) ? cost.ToString() : "-";
        levelText.text = level.ToString();

        // 가능 여부에 따라 비용 색상 변경
        costText.color = playerStat.CanUpgrade(statType) ? Color.black : Color.red;
    }

    /// <summary>
    /// 버튼을 누른 상태일 때 호출됩니다. 롱클릭 강화를 시작합니다.
    /// </summary>
    public void OnPointerDown()
    {
        isHolding = true;
        holdCoroutine = StartCoroutine(HoldUpgradeRoutine());
    }

    /// <summary>
    /// 버튼에서 손을 뗐을 때 호출됩니다. 롱클릭 강화를 종료합니다.
    /// </summary>
    public void OnPointerUp()
    {
        isHolding = false;

        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
    }

    /// <summary>
    /// 롱클릭 동안 일정 간격으로 반복 강화 실행을 처리하는 코루틴입니다.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator HoldUpgradeRoutine()
    {
        yield return new WaitForSeconds(0.3f);        // 롱클릭 시작 지연

        while (isHolding)
        {
            playerStat.UpgradeStat(statType);        // 강화 실행
            RefreshUI();                             // UI 반영
            yield return new WaitForSeconds(0.1f);   // 반복 간격
        }
    }
}